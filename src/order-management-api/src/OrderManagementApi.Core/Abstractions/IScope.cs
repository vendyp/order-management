namespace OrderManagementApi.Core.Abstractions;

public interface IScope
{
    string ScopeName { get;  }
    bool ExcludeResult { get; }
}