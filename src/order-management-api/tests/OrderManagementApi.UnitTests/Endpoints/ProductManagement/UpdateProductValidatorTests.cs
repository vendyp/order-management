using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.UnitTests.Builders;
using OrderManagementApi.WebApi.Endpoints.ProductManagement;
using Shouldly;

namespace OrderManagementApi.UnitTests.Endpoints.ProductManagement;

public class UpdateProductValidatorTests
{
    public static IEnumerable<object[]> GetInvalidRequests()
    {
        yield return new object[]
        {
            new UpdateProductRequest
            {
                Payload = new UpdateProductRequestPayload
                {
                    Name = "Stainless Steel Wood Holder",
                    Description = null,
                    File = "test.jpg",
                    Price = -200m
                }
            }
        };

        yield return new object[]
        {
            new UpdateProductRequest
            {
                Payload = new UpdateProductRequestPayload
                {
                    Name = "ステンレススティック",
                    Description = null,
                    File = Guid.NewGuid().ToString(),
                    Price = 152m
                }
            }
        };

        yield return new object[]
        {
            new UpdateProductRequest
            {
                Payload = new UpdateProductRequestPayload
                {
                    Name = "不锈钢棒",
                    Description = null,
                    File = Guid.NewGuid().ToString(),
                    Price = 152m
                }
            }
        };

        yield return new object[]
        {
            new UpdateProductRequest
            {
                Payload = new UpdateProductRequestPayload
                {
                    Name = "عصا غير قابلة للصدأ",
                    Description = null,
                    File = Guid.NewGuid().ToString(),
                    Price = 152m
                }
            }
        };
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public async Task UpdateProduct_Given_Invalid_Request_Should_Return_BadRequest(UpdateProductRequest request)
    {
        var handler = new UpdateProduct(DbContextBuilder.Create().Object, new Mock<IFileRepository>().Object);

        var result = await handler.HandleAsync(request, CancellationToken.None);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<BadRequestObjectResult>();
    }
}