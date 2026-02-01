namespace Ordering.Core.Entities;

public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAtUtc { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedAtUtc { get; set; }
    
}