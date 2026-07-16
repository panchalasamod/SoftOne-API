using SoftOne.Models.Base;

namespace SoftOne.Models.Requests;

public class TaskAddRequest : BaseAddRequest
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int Priority { get; set; } = 1;

    public DateTime? DueDate { get; set; }

    public bool IsCompleted { get; set; }

    public bool IsReOpened { get; set; }
}
