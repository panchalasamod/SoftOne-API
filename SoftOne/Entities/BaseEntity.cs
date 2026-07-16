using System.ComponentModel.DataAnnotations;

namespace SoftOne.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }

    public int? CreatedUserId { get; set; }

    public int? UpdatedUserId { get; set; }

    public bool IsDeleted { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
