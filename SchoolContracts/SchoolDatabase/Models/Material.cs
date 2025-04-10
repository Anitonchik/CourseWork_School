using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolDatabase.Models;

public class Material
{
    public required string Id { get; set; } = Guid.NewGuid().ToString();
    public required string StorekeeperId { get; set; }
    public required string MaterialName { get; set; }
    public string Description { get; set; }
    public Storekeeper? Storekeeper { get; set; }

    [ForeignKey("MaterialId")]
    public List<CircleMaterial>? CircleMaterials { get; set; }

    [ForeignKey("MaterialId")]
    public List<InterestMaterial>? InterestMaterials { get; set; }
}
