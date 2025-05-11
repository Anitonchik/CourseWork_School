namespace SchoolWebApi.Models;

public class LoginRequestModel
{
    public string? UserName { get; set; }
    public UserRole? Role { get; set; }
    public string? Password { get; set; }
}
