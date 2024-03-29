﻿using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Enums;
using OrderManagementApi.WebApi.Endpoints.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace OrderManagementApi.IntegrationTests.Endpoints.UserManagement;

[Collection(nameof(UserManagementFixture))]
public class SetUserInActiveTests
{
    private readonly UserManagementFixture _serviceFixture;

    public SetUserInActiveTests(UserManagementFixture serviceFixture)
    {
        _serviceFixture = serviceFixture;
    }

    [Fact]
    public async Task SetUserInActive_Should_Do_Correctly()
    {
        // Arrange
        using var scope = _serviceFixture.ServiceProvider.CreateScope();

        var request = new SetUserInActiveRequest
        {
            UserId = Guid.Empty
        };

        var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();

        //first time get
        var user = await dbContext.Set<User>().Where(e => e.UserId == request.UserId)
            .FirstOrDefaultAsync(CancellationToken.None);
        user.ShouldNotBeNull();
        user.StatusRecord.ShouldBe(StatusRecord.Active);

        var setUserInActive = new SetUserInActive(dbContext);

        var result = await setUserInActive.HandleAsync(request, CancellationToken.None);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(NoContentResult));

        //second time get
        user = await dbContext.Set<User>().Where(e => e.UserId == request.UserId)
            .FirstOrDefaultAsync(CancellationToken.None);
        user.ShouldNotBeNull();
        user.StatusRecord.ShouldBe(StatusRecord.InActive);
    }
}