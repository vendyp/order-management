﻿namespace OrderManagementApi.WebApi.Endpoints.Identity;

public class ChangePasswordRequest
{
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
}