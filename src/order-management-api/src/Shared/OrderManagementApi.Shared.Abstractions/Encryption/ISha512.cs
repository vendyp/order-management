﻿namespace OrderManagementApi.Shared.Abstractions.Encryption;

public interface ISha512
{
    string Hash(string data);
}