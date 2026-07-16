namespace SoftOne.Models.Base;

public abstract class BaseDTO
{
    public int Id { get; set; }

    public int? CreatedUserId { get; set; }

    public int? UpdatedUserId { get; set; }

    public bool IsDeleted { get; set; }

    /// <summary>
    /// Base64-encoded concurrency token for optimistic concurrency checks.
    /// </summary>
    public string? RowVersion { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
