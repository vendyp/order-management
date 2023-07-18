namespace OrderManagementApi.Core.Modules;

public class Module
{
    private Module()
    {
        DisplayName = string.Empty;
        ItemModules = null;
        Modules = null;
    }

    public Module(string displayName, List<ItemModule> itemModules) : this()
    {
        DisplayName = displayName;
        ItemModules = itemModules;
    }

    public Module(string displayName, List<Module> modules) : this()
    {
        DisplayName = displayName;
        Modules = modules;
    }

    public string DisplayName { get; set; }
    public List<ItemModule>? ItemModules { get; }
    public List<Module>? Modules { get; }
}