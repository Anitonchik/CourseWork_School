using AutoMapper;
using SchoolContracts.DataModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolDatabase.Models;

[AutoMap(typeof(CircleDataModel), ReverseMap = true)]
public class Circle
{
    [Key]
    public required string Id { get; set; } = Guid.NewGuid().ToString();
    public required string StorekeeperId { get; set; }
    public required string CircleName { get; set; }
    public string Description { get; set; }
    public Storekeeper? Storekeeper { get; set; }

    [ForeignKey("CircleId")]
    public List<CircleMaterial>? CircleMaterials { get; set; }

    [ForeignKey("CircleId")]
    public List<LessonCircle>? LessonCircles { get; set; }
}
