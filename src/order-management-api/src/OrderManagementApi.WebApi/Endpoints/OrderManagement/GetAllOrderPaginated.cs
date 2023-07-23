using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Queries;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Dto;
using OrderManagementApi.WebApi.Endpoints.OrderManagement.Scopes;
using OrderManagementApi.WebApi.Shared.Validators;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.OrderManagement;

public class GetAllOrderPaginated : BaseEndpoint<GetAllOrderPaginatedRequest, PagedList<OrderManagementDto>>
{
    private readonly IDbContext _dbContext;

    public GetAllOrderPaginated(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("orders")]
    [Authorize]
    [RequiredScope(typeof(OrderManagementScope))]
    [SwaggerOperation(
        Summary = "Get All Order API",
        Description = "",
        OperationId = "OrderManagement.GetAllProductPaginated",
        Tags = new[] { "OrderManagement" })
    ]
    [ProducesResponseType(typeof(PagedList<ProductManagementDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<PagedList<OrderManagementDto>>> HandleAsync(
        [FromQuery] GetAllOrderPaginatedRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new GetAllOrderPaginatedRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        var queryable = _dbContext.Set<Order>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Number) && request.Number.Length > 2)
            queryable = queryable.Where(e => EF.Functions.ILike(e.Number, $"%{request.Number}%"));

        if (request.OrderStartDate.HasValue)
            queryable = queryable.Where(e => e.CreatedAt!.Value.Date >= request.OrderStartDate.Value.Date);

        if (request.OrderEndDate.HasValue)
            queryable = queryable.Where(e => e.CreatedAt!.Value.Date <= request.OrderEndDate.Value.Date);

        var results = await queryable
            .GroupBy(e => new { e.OrderId, e.Number, e.TotalPrice, e.Status })
            .Select(e => new OrderManagementDto
            {
                OrderId = e.Key.OrderId,
                Number = e.Key.Number,
                TotalPrice = e.Key.TotalPrice,
                Status = e.Key.Status,
                TotalItems = e.Count()
            })
            .Skip(request.CalculateSkip())
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        return new PagedList<OrderManagementDto>(results, request.Page, request.Size);
    }
}