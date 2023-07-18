using OrderManagementApi.Shared.Abstractions.Queries;

namespace OrderManagementApi.WebApi.Endpoints.UserManagement;

public class GetAllUserPaginatedRequest : BasePaginationCalculation
{
    public string? Username { get; set; }
    public string? Fullname { get; set; }
}