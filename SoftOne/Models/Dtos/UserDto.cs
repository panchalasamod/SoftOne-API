using SoftOne.Models.Base;

namespace SoftOne.Models.Dtos;

public class UserDto : BaseDTO
{
    public string Username { get; set; } = string.Empty;
}
