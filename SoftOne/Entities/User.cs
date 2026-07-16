using System.ComponentModel.DataAnnotations;

namespace SoftOne.Entities;

public class User : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string PasswordHash { get; set; } = string.Empty;
}
