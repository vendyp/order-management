namespace OrderManagementApi.WebApi.Shared.Abstractions;

public interface IScope
{
    string ScopeName { get;  }
    bool ExcludeResult { get; }
}