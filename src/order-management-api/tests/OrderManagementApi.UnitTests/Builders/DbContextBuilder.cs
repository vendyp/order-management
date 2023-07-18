using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Entities;
using MockQueryable.Moq;
using Moq;

namespace OrderManagementApi.UnitTests.Builders;

public static class DbContextBuilder
{
    public static Mock<IDbContext> Create()
    {
        var mock = new Mock<IDbContext>();

        return mock;
    }

    public static void Setup<T>(this Mock<IDbContext> dbContextMock,
        IEnumerable<T> expectedReturns)
        where T : BaseEntity
    {
        dbContextMock.Setup(e => e.Set<T>()).Returns(expectedReturns.AsQueryable().BuildMockDbSet().Object);
    }
}