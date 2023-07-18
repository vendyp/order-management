using OrderManagementApi.Domain;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Domain.Extensions;
using OrderManagementApi.Shared.Abstractions.Clock;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;

namespace OrderManagementApi.Core;

public class CoreInitializer : IInitializer
{
    private readonly IDbContext _dbContext;
    private readonly ISalter _salter;
    private readonly IRng _rng;
    private readonly IClock _clock;

    public CoreInitializer(IDbContext dbContext, ISalter salter, IRng rng, IClock clock)
    {
        _dbContext = dbContext;
        _salter = salter;
        _rng = rng;
        _clock = clock;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await AddSuperAdministratorRoleAsync(cancellationToken);

        await AddAdministratorRoleAsync(cancellationToken);

        await AddSuperAdministratorAsync(cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task AddAdministratorRoleAsync(CancellationToken cancellationToken)
    {
        if (await _dbContext.Set<Role>().AnyAsync(e => e.RoleId == RoleExtensions.AdministratorId,
                cancellationToken: cancellationToken))
            return;

        var role = new Role(RoleExtensions.AdministratorId, RoleExtensions.Administrator);

        _dbContext.Insert(role);
    }

    private async Task AddSuperAdministratorRoleAsync(CancellationToken cancellationToken)
    {
        if (await _dbContext.Set<Role>().AnyAsync(e => e.RoleId == RoleExtensions.SuperAdministratorId,
                cancellationToken: cancellationToken))
            return;

        var role = new Role(RoleExtensions.SuperAdministratorId, RoleExtensions.SuperAdministrator);

        _dbContext.Insert(role);
    }

    private async Task AddSuperAdministratorAsync(CancellationToken cancellationToken)
    {
        if (await _dbContext.Set<User>().AnyAsync(e => e.UserId == DefaultUser.SuperAdministratorId,
                cancellationToken: cancellationToken))
            return;

        var user = DefaultUser.SuperAdministrator(_rng, _salter, _clock);

        _dbContext.Insert(user);
    }
}