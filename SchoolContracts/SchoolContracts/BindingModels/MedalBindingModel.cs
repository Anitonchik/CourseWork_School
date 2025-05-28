namespace SchoolContracts.BindingModels;

public class MedalBindingModel
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();
    public string? StorekeeperId { get; set; }
    public string? MaterialId { get; set; }
    public string? MedalName { get; set; }
    public int Range { get; set; }
    public string? Description { get; set; }
}
