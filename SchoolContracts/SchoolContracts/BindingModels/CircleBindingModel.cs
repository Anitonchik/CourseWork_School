namespace SchoolContracts.BindingModels;

public class CircleBindingModel
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();
    public string? StorekeeperId { get; set; }
    public string CircleName { get; set; }
    public string Description { get; set; }
}
