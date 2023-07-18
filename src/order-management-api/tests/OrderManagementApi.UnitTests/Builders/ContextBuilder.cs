﻿using OrderManagementApi.Shared.Abstractions.Contexts;
using Moq;

namespace OrderManagementApi.UnitTests.Builders;

public static class ContextBuilder
{
    public static Mock<IContext> Create()
    {
        var mock = new Mock<IContext>();
        mock.Setup(e => e.RequestId).Returns(Guid.NewGuid);
        mock.Setup(e => e.TraceId).Returns(Guid.NewGuid().ToString);
        mock.Setup(e => e.IpAddress).Returns((string?)null);
        mock.Setup(e => e.UserAgent).Returns((string?)null);

        var test = new Mock<IIdentityContext>();
        test.Setup(e => e.IsAuthenticated).Returns(false);
        test.Setup(e => e.Id).Returns(Guid.NewGuid);
        test.Setup(e => e.Username).Returns(string.Empty);
        test.Setup(e => e.Claims).Returns(new Dictionary<string, IEnumerable<string>>());
        test.Setup(e => e.Roles).Returns(new List<string>());
        mock.Setup(e => e.Identity).Returns(test.Object);

        return mock;
    }
}