using SoftOne.Entities;
using SoftOne.Models.Requests;
using SoftOne.Models.Responses;
using SoftOne.Repositories;

namespace SoftOne.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await ValidateCredentialsAsync(request.Username, request.Password, cancellationToken);
        if (user is null)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid username or password."
            };
        }

        return new LoginResponse
        {
            Success = true,
            UserId = user.Id,
            Username = user.Username,
            Message = "Login successful."
        };
    }

    public async Task<User?> ValidateCredentialsAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        var user = await _userRepository.GetByUsernameAsync(username.Trim(), cancellationToken);
        if (user is null)
        {
            return null;
        }

        return _passwordHasher.Verify(password, user.PasswordHash) ? user : null;
    }
}
