namespace SchoolWebApi.Models;

public class LoginResponseModel
{
    public string? Id {  get; set; }
    public string? Role {  get; set; }
    public string? UserLogin {  get; set; }
    public string? UserFIO {  get; set; }
    public string? Mail {  get; set; }
    public string? AccessToken { get; set; }
    public int ExpiresIn { get; set; }
}
