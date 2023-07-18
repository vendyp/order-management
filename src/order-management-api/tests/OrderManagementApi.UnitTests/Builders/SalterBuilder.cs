using OrderManagementApi.Shared.Abstractions.Encryption;
using Moq;

namespace OrderManagementApi.UnitTests.Builders;

public static class SalterBuilder
{
    public static Mock<ISalter> Create()
    {
        return new Mock<ISalter>();
    }
}