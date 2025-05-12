namespace SchoolDatabase.Models;

public class InterestMaterial
{
    public required string InterestId { get; set; }
    public required string MaterialId { get; set; }
    public Interest? Interest { get; set; }
    public Material? Material { get; set; }
}
