namespace SchoolDatabase.Models;

public class CircleMaterial
{
    public required string CircleId { get; set; }
    public required string MaterialId { get; set; }
    public int Count { get; set; }
    public Circle? Circle { get; set; }
    public Material? Material { get; set; }
}
