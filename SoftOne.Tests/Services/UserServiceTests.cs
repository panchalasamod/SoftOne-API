using FluentAssertions;
using Moq;
using SoftOne.Entities;
using SoftOne.Models.Requests;
using SoftOne.Repositories;
using SoftOne.Services;

namespace SoftOne.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly UserService _sut;

    public UserServiceTests()
    {
        _sut = new UserService(_userRepository.Object, _passwordHasher.Object);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsSuccess()
    {
        var user = new User { Id = 1, Username = "admin", PasswordHash = "hash" };
        _userRepository
            .Setup(r => r.GetByUsernameAsync("admin", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _passwordHasher.Setup(h => h.Verify("admin123", "hash")).Returns(true);

        var result = await _sut.LoginAsync(new LoginRequest
        {
            Username = "admin",
            Password = "admin123"
        });

        result.Success.Should().BeTrue();
        result.UserId.Should().Be(1);
        result.Username.Should().Be("admin");
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ReturnsFailure()
    {
        var user = new User { Id = 1, Username = "admin", PasswordHash = "hash" };
        _userRepository
            .Setup(r => r.GetByUsernameAsync("admin", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _passwordHasher.Setup(h => h.Verify("bad", "hash")).Returns(false);

        var result = await _sut.LoginAsync(new LoginRequest
        {
            Username = "admin",
            Password = "bad"
        });

        result.Success.Should().BeFalse();
        result.Message.Should().Contain("Invalid");
    }

    [Fact]
    public async Task ValidateCredentialsAsync_WithEmptyUsername_ReturnsNull()
    {
        var result = await _sut.ValidateCredentialsAsync(" ", "password");
        result.Should().BeNull();
    }
}
