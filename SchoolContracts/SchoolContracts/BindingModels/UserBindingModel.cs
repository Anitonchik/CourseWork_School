namespace SchoolContracts.BindingModels;

public class UserBindingModel
{
    public string? Id { get; set; }
    public string? FIO { get; set; }
    public string? Login { get; set; }
    public int? Role { get; set; }
    public string? Password { get; set; }
    public string? Mail { get; set; }
}
