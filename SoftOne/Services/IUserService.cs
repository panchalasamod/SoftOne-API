using SoftOne.Entities;
using SoftOne.Models.Requests;
using SoftOne.Models.Responses;

namespace SoftOne.Services;

public interface IUserService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

    Task<User?> ValidateCredentialsAsync(string username, string password, CancellationToken cancellationToken = default);
}
