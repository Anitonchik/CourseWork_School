namespace SchoolDatabase.Models;

public class Medal
{
    public required string Id { get; set; }
    public required string StorekeeperId { get; set; }
    public required string MaterialId { get; set; }
    public required string MedalName { get; set; }
    public int Range { get; set; }
    public string Description { get; set; }
    public Storekeeper? Storekeeper { get; set; }
    public Material? Material { get; set; }
}
