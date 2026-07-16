using System.ComponentModel.DataAnnotations;

namespace SoftOne.Models.Base;

public abstract class BaseUpdateRequest
{
    [Required]
    public int Id { get; set; }

    /// <summary>
    /// Base64-encoded concurrency token.
    /// </summary>
    [Required]
    public string RowVersion { get; set; } = string.Empty;
}
