using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using SoftOne.Services;

namespace SoftOne.Auth;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUserService _userService;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IUserService userService)
        : base(options, logger, encoder)
    {
        _userService = userService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Missing Authorization header.");
        }

        try
        {
            var header = AuthenticationHeaderValue.Parse(Request.Headers.Authorization!);
            if (!string.Equals(header.Scheme, "Basic", StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(header.Parameter))
            {
                return AuthenticateResult.Fail("Invalid Authorization scheme.");
            }

            var credentialBytes = Convert.FromBase64String(header.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
            if (credentials.Length != 2)
            {
                return AuthenticateResult.Fail("Invalid Authorization header format.");
            }

            var username = credentials[0];
            var password = credentials[1];

            var user = await _userService.ValidateCredentialsAsync(username, password);
            if (user is null)
            {
                return AuthenticateResult.Fail("Invalid username or password.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserId", user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Asuthentication failed.");
            return AuthenticateResult.Fail("Invalid Authorization header.");
        }
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers.WWWAuthenticate = "Basic realm=\"SoftOne Tasks API\"";
        return base.HandleChallengeAsync(properties);
    }
}
