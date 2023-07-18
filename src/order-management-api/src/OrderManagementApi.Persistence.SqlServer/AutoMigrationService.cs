using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagementApi.Shared.Abstractions.Databases;

namespace OrderManagementApi.Persistence.SqlServer;

public class AutoMigrationService : IInitializer
{
    private readonly SqlServerDbContext _dbContext;
    private readonly ILogger<AutoMigrationService> _logger;

    public AutoMigrationService(SqlServerDbContext dbContext, ILogger<AutoMigrationService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception occurs when AutoMigrationService running");
            throw;
        }
    }
}