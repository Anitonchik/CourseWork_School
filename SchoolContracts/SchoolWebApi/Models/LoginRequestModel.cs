namespace SchoolWebApi.Models;

public class LoginRequestModel
{
    public string? UserLogin { get; set; }
    public string? Role { get; set; }
    public string? Password { get; set; }
}
