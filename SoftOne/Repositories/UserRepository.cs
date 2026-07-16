using Microsoft.EntityFrameworkCore;
using SoftOne.Data;
using SoftOne.Entities;

namespace SoftOne.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(
            u => u.Username.ToLower() == username.ToLower(),
            cancellationToken);
    }
}
