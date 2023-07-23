﻿using Microsoft.AspNetCore.Mvc;

namespace OrderManagementApi.WebApi.Endpoints.ProductManagement;

public class DeleteProductRequest
{
    [FromRoute(Name = "productId")] public Guid ProductId { get; set; }
}