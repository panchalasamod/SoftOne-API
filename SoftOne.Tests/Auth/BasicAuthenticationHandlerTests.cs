using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using SoftOne.Auth;
using SoftOne.Entities;
using SoftOne.Services;

namespace SoftOne.Tests.Auth;

public class BasicAuthenticationHandlerTests
{
    private readonly Mock<IUserService> _userService = new();

    private async Task<AuthenticateResult> AuthenticateAsync(string? authorizationHeader)
    {
        var options = new Mock<IOptionsMonitor<AuthenticationSchemeOptions>>();
        options.Setup(o => o.Get(It.IsAny<string>())).Returns(new AuthenticationSchemeOptions());

        var handler = new BasicAuthenticationHandler(
            options.Object,
            NullLoggerFactory.Instance,
            UrlEncoder.Default,
            _userService.Object);

        var context = new DefaultHttpContext();
        if (authorizationHeader is not null)
        {
            context.Request.Headers.Authorization = authorizationHeader;
        }

        await handler.InitializeAsync(
            new AuthenticationScheme("Basic", "Basic", typeof(BasicAuthenticationHandler)),
            context);

        return await handler.AuthenticateAsync();
    }

    [Fact]
    public async Task Authenticate_WithoutHeader_Fails()
    {
        var result = await AuthenticateAsync(null);
        result.Succeeded.Should().BeFalse();
        result.Failure!.Message.Should().Contain("Missing");
    }

    [Fact]
    public async Task Authenticate_WithValidCredentials_Succeeds()
    {
        _userService
            .Setup(s => s.ValidateCredentialsAsync("admin", "admin123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { Id = 7, Username = "admin" });

        var token = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:admin123"));
        var result = await AuthenticateAsync($"Basic {token}");

        result.Succeeded.Should().BeTrue();
        var principal = result.Principal;
        principal.Should().NotBeNull();
        principal!.FindFirstValue("UserId").Should().Be("7");

        var identity = principal.Identity;
        identity.Should().NotBeNull();
        identity!.Name.Should().Be("admin");
    }

    [Fact]
    public async Task Authenticate_WithInvalidCredentials_Fails()
    {
        _userService
            .Setup(s => s.ValidateCredentialsAsync("admin", "bad", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var token = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:bad"));
        var result = await AuthenticateAsync($"Basic {token}");

        result.Succeeded.Should().BeFalse();
    }
}
