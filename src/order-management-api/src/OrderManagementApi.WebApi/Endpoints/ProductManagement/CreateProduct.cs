using Microsoft.AspNetCore.Mvc;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.WebApi.Validators;

namespace OrderManagementApi.WebApi.Endpoints.ProductManagement;

public class CreateProduct : BaseEndpointWithoutResponse<CreateProductRequest>
{
    private readonly IDbContext _dbContext;

    public CreateProduct(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<ActionResult> HandleAsync(CreateProductRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new CreateProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        throw new NotImplementedException();
    }
}