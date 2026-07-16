namespace SoftOne.Models.Responses;

public class LoginResponse
{
    public int UserId { get; set; }

    public string Username { get; set; } = string.Empty;

    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;
}
