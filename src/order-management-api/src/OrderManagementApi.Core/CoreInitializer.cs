using OrderManagementApi.Domain;
using OrderManagementApi.Domain.Entities;
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
        await AddSuperAdministratorAsync(cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
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