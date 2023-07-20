using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.WebApi.Endpoints.ProductManagement;
using Shouldly;

namespace OrderManagementApi.UnitTests.Endpoints.ProductManagement;

public class CreateProductTests
{
    [Fact]
    public async Task CreateProduct_Given_Correct_Request_Should_Do_As_Expected()
    {
        var request = new CreateProductRequest
        {
            Name = "Stainless Steel Stick",
            Description = "Very powerful and light stick made using steel",
            File = Guid.NewGuid().ToString(),
            Price = 199m
        };

        var dbContext = new Mock<IDbContext>();
        var productId = Guid.NewGuid();
        dbContext.Setup(e => e.Insert(It.IsAny<Product>()))
            .Callback<Product>(entity =>
            {
                productId = entity.ProductId;
                entity.Name.ShouldBe(request.Name);
                entity.Description.ShouldBe(request.Description);
                entity.Price.ShouldBe(request.Price!.Value);
            });

        var fileRepositoryMock = new Mock<IFileRepository>();

        //because class if ref
        var fileRepository = new FileRepository();
        fileRepositoryMock.Setup(e => e.GetFileBydIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fileRepository);

        var ctor = new CreateProduct(dbContext.Object, fileRepositoryMock.Object);

        var result = await ctor.HandleAsync(request, CancellationToken.None);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<NoContentResult>();

        fileRepository.Source.ShouldBe(productId.ToString());

        dbContext.Verify(e => e.Insert(It.IsAny<Product>()), Times.Once);
        dbContext.Verify(e => e.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}