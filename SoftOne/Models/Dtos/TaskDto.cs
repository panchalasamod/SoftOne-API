using SoftOne.Models.Base;

namespace SoftOne.Models.Dtos;

public class TaskDto : BaseDTO
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int Priority { get; set; }

    public DateTime? DueDate { get; set; }

    public bool IsCompleted { get; set; }

    public bool IsReOpened { get; set; }
}
