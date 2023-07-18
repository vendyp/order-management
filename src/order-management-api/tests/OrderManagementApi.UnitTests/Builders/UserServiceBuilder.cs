using OrderManagementApi.Core.Abstractions;
using Moq;

namespace OrderManagementApi.UnitTests.Builders;

public static class UserServiceBuilder
{
    public static Mock<IUserService> Create()
    {
        return new Mock<IUserService>();
    }
}