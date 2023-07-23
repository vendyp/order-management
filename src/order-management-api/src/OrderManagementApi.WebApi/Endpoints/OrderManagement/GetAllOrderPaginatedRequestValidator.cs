using FluentValidation;

namespace OrderManagementApi.WebApi.Endpoints.OrderManagement;

public class GetAllOrderPaginatedRequestValidator : AbstractValidator<GetAllOrderPaginatedRequest>
{
    public GetAllOrderPaginatedRequestValidator()
    {
        RuleFor(e => e.OrderStartDate).GreaterThan(e => e.OrderEndDate);
    }
}