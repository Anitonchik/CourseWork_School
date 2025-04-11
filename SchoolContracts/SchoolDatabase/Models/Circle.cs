using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolDatabase.Models;

public class Circle
{
    public required string Id { get; set; } = Guid.NewGuid().ToString();
    public required string StorekeeperId { get; set; }
    public required string CircleName { get; set; }
    public string Description { get; set; }
    public Storekeeper? Storekeeper { get; set; }

    [ForeignKey("CircleId")]
    public List<CircleMaterial>? CircleMaterials { get; set; }

    [ForeignKey("CircleId")]
    public List<LessonCircle>? LessonCircles { get; set; }

    [ForeignKey("CircleId")]
    public List<InterestMaterial>? InterestMaterials { get; set; }
}
