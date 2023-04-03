namespace OrderManagement.Abstractions;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        IsDeleted = false;
    }

    public string? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime? CreatedByAt { get; set; }
    public DateTime? CreatedByAtServer { get; set; }

    public string? LastUpdatedBy { get; set; }
    public string? LastUpdatedByName { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public DateTime? LastUpdatedAtServer { get; set; }

    /// <summary>
    /// Default value is false.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Default value is Active.
    /// </summary>
    public DataState DataState { get; set; }

    public void SetToInActive()
    {
        DataState = DataState.InActive;
    }

    public void SetToActive()
    {
        DataState = DataState.Active;
    }
}