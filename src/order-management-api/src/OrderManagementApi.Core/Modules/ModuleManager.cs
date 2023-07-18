namespace OrderManagementApi.Core.Modules;

/// <summary>
/// When update this, please note that every module or sub module,
///
/// Frontend must update it too.
/// </summary>
public class ModuleManager
{
    private readonly List<Module> _modules;

    public ModuleManager()
    {
        _names = new List<string>();
        _modules = new List<Module>();

        ConstructModules();
    }

    /// <summary>
    /// Construct in order
    /// </summary>
    private void ConstructModules()
    {
        DashboardModule();
        UserManagementModule();
        RoleManagementModule();
    }

    private void RoleManagementModule()
    {
        var itemModules = new List<ItemModule>
        {
            new("rolemanagement.view", "View"),
            new("rolemanagement.create", "Create"),
            new("rolemanagement.edit", "Edit")
        };

        var module = new Module("Role Management", itemModules);
        _modules.Add(module);
    }

    private void UserManagementModule()
    {
        var itemModules = new List<ItemModule>
        {
            new("usermanagement.view", "View"),
            new("usermanagement.create", "Create"),
            new("usermanagement.edit", "Edit")
        };

        var module = new Module("User Management", itemModules);
        _modules.Add(module);
    }

    private void DashboardModule()
    {
        var itemModules = new List<ItemModule> { new("dashboard.view", "View") };
        var module = new Module("Dashboard", itemModules);
        _modules.Add(module);
    }

    public List<Module> GetAllModules() => _modules;

    private readonly List<string> _names;

    /// <summary>
    /// Deep level support only 1
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllModuleName()
    {
        if (_names.Any()) return _names;

        foreach (var item in _modules)
        {
            if (item.ItemModules != null)
                _names.AddRange(item.ItemModules.Select(e => e.Name));

            if (item.Modules != null)
                _names.AddRange(item.Modules.SelectMany(e => e.ItemModules!).Select(e => e.Name));
        }

        return _names;
    }
}