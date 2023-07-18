using OrderManagementApi.Shared.Abstractions.Encryption;
using Moq;

namespace OrderManagementApi.UnitTests.Builders;

public static class RngBuilder
{
    public static Mock<IRng> Create()
    {
        return new Mock<IRng>();
    }
}