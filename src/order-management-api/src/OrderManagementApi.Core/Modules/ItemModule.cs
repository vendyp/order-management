namespace OrderManagementApi.Core.Modules;

public class ItemModule
{
    public ItemModule(string name, string displayName)
    {
        Name = name;
        DisplayName = displayName;
    }

    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string? Description { get; set; }
}