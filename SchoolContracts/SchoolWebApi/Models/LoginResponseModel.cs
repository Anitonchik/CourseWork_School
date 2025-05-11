namespace SchoolWebApi.Models;

public class LoginResponseModel
{
    public string? UserLogin {  get; set; }
    public string? AccessToken { get; set; }
    public int ExpiresIn { get; set; }
}
