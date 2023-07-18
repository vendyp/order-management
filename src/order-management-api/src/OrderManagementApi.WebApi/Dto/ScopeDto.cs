namespace OrderManagementApi.WebApi.Dto;

public class ScopeDto
{
    public ScopeDto(string name)
    {
        Name = name;
    }
    
    public string Name { get; set; }
}