using Microsoft.AspNetCore.Mvc;

namespace OrderManagementApi.WebApi.Endpoints.Sample;

public class GetCacheRequest
{
    [FromRoute(Name = "key")] public string Key { get; set; } = null!;
}