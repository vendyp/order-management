﻿using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace OrderManagementApi.WebApi.HealthChecks;

public class ApplicationHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        return Task.FromResult(HealthCheckResult.Healthy("Application run perfectly.."));
    }
}