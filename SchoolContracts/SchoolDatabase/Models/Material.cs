using AutoMapper;
using SchoolContracts.DataModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolDatabase.Models;

[AutoMap(typeof(MaterialDataModel), ReverseMap = true)]
public class Material
{
    [Key]
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
