using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;

namespace OrderManagementApi.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IDbContext _dbContext;
    private readonly ISalter _salter;

    public UserService(IDbContext dbContext, ISalter salter)
    {
        _dbContext = dbContext;
        _salter = salter;
    }

    public IQueryable<User> GetBaseUserQuery() => _dbContext.Set<User>()
        .Include(e => e.UserScopes)
        .AsQueryable();

    public Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        => GetBaseUserQuery()
            .Where(e => e.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
        => GetBaseUserQuery()
            .Where(e => e.NormalizedUsername == username.ToUpper())
            .FirstOrDefaultAsync(cancellationToken);

    public Task<bool> IsUsernameExistAsync(string username, CancellationToken cancellationToken)
    {
        username = username.ToUpper();

        return _dbContext.Set<User>().Where(e => e.NormalizedUsername == username)
            .AnyAsync(cancellationToken);
    }

    public bool VerifyPassword(string currentPassword, string salt, string password)
        => currentPassword == _salter.Hash(salt, password);
}