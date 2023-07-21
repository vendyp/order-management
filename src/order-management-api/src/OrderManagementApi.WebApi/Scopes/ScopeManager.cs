using OrderManagementApi.WebApi.Shared.Abstractions;

namespace OrderManagementApi.WebApi.Scopes;

public class ScopeManager : IScopeManager
{
    private readonly List<string> _list;

    public ScopeManager()
    {
        _list = new List<string>();
        var typeOfIScope = typeof(IScope);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeOfIScope.IsAssignableFrom(p) && p != typeOfIScope)
            .ToList();
        foreach (var item in types)
        {
            var instance = Activator.CreateInstance(item)!;
            var propertyInfoName = item.GetProperty(nameof(IScope.ScopeName))!;
            _list.Add((propertyInfoName.GetValue(instance)! as string)!);
        }
    }

    public List<string> GetAllScopes() => _list;

    private static readonly Lazy<ScopeManager> Lazy = new(() => new ScopeManager());

    public static ScopeManager Instance => Lazy.Value;
}