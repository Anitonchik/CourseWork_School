namespace SchoolContracts.BindingModels;

public class UserBindingModel
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();
    public string? FIO { get; set; }
    public string? Login { get; set; }
    public string? Role { get; set; }
    public string? Password { get; set; }
    public string? Mail { get; set; }
}
