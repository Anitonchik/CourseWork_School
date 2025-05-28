namespace SchoolContracts.BindingModels;

public class MaterialBindingModel
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();
    public string? StorekeeperId { get; set; }
    public string? MaterialName { get; set; }
    public string? Description { get; set; }
}
