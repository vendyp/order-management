using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementApi.UnitTests.Builders;
using OrderManagementApi.WebApi.Endpoints.ProductManagement;
using Shouldly;

namespace OrderManagementApi.UnitTests.Endpoints.ProductManagement;

public class CreateProductValidatorTests
{
    public static IEnumerable<object[]> GetInvalidRequests()
    {
        yield return new object[]
        {
            new CreateProductRequest(),
        };

        yield return new object[]
        {
            new CreateProductRequest
            {
                Name = "Stainless Steel Wood Holder",
                Description = null,
                File = "test.jpg",
                Price = -200m
            },
        };

        yield return new object[]
        {
            new CreateProductRequest
            {
                Name = "Stainless Steel Wood Holder",
                Description = null,
                File = null,
                Price = 152m
            },
        };

        yield return new object[]
        {
            new CreateProductRequest
            {
                Name = "Stainless Steel Wood Holder",
                Description = null,
                File = null,
                Price = 152m
            },
        };

        yield return new object[]
        {
            new CreateProductRequest
            {
                Name = "ステンレススティック",
                Description = null,
                File = Guid.NewGuid().ToString(),
                Price = 152m
            },
        };

        yield return new object[]
        {
            new CreateProductRequest
            {
                Name = "不锈钢棒",
                Description = null,
                File = Guid.NewGuid().ToString(),
                Price = 152m
            },
        };

        yield return new object[]
        {
            new CreateProductRequest
            {
                Name = "عصا غير قابلة للصدأ",
                Description = null,
                File = Guid.NewGuid().ToString(),
                Price = 152m
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public async Task CreateProduct_Given_Invalid_Request_Should_Return_BadRequest(CreateProductRequest request)
    {
        var handler = new CreateProduct(DbContextBuilder.Create().Object);

        var result = await handler.HandleAsync(request, CancellationToken.None);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<BadRequestObjectResult>();
    }
}