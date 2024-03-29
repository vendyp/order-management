﻿using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Clock;
using OrderManagementApi.Shared.Abstractions.Encryption;

namespace OrderManagementApi.Domain;

public static class DefaultUser
{
    public static Guid SuperAdministratorId => Guid.Empty;

    /// <summary>
    /// This SuperAdministrator is default user value created with
    /// Username : admin
    /// Password : Qwerty@1234 
    /// </summary>
    /// <param name="rng">Random generator service</param>
    /// <param name="salter">Salter service</param>
    /// <param name="clock">Clock service</param>
    /// <returns></returns>
    public static User SuperAdministrator(IRng rng, ISalter salter, IClock clock)
    {
        var salt = rng.Generate(128, false);

        var newUser = new User
        {
            UserId = SuperAdministratorId,
            Username = "admin",
            NormalizedUsername = "admin".ToUpper(),
            Salt = salt,
            Password = salter.Hash(salt, "Qwerty@1234"),
            LastPasswordChangeAt = clock.CurrentDate(),
            FullName = "Super Administrator",
            CreatedBy = Guid.Empty.ToString(),
            CreatedByName = "system",
            CreatedAt = clock.CurrentDate(),
            CreatedAtServer = clock.CurrentServerDate()
        };

        return newUser;
    }
}