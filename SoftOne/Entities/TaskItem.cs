using System.ComponentModel.DataAnnotations;

namespace SoftOne.Entities;

public class TaskItem : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    public int Priority { get; set; } = 1;

    public DateTime? DueDate { get; set; }

    public bool IsCompleted { get; set; }
    public bool IsReOpened { get; set; }
}
