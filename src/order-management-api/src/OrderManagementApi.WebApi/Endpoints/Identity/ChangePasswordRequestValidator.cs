﻿using FluentValidation;

namespace OrderManagementApi.WebApi.Endpoints.Identity;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(e => e.CurrentPassword).NotNull().NotEmpty().MaximumLength(256);
        RuleFor(e => e.NewPassword).NotNull().NotEmpty().MaximumLength(256);
        RuleFor(e => e.NewPassword).NotEqual(e => e.CurrentPassword);
    }
}