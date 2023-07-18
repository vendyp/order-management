using Microsoft.AspNetCore.Mvc;

namespace OrderManagementApi.WebApi.Endpoints.Sample;

public class DeleteCacheRequest
{
    [FromRoute(Name = "key")] public string Key { get; set; } = null!;
}