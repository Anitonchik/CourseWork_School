using AutoMapper;
using SchoolContracts.DataModels;
using System.ComponentModel.DataAnnotations;

namespace SchoolDatabase.Models;

[AutoMap(typeof(MedalDataModel), ReverseMap = true)]
public class Medal
{
    [Key]
    public required string Id { get; set; }
    public required string StorekeeperId { get; set; }
    public required string MaterialId { get; set; }
    public required string MedalName { get; set; }
    public int Range { get; set; }
    public string Description { get; set; }
    public Storekeeper? Storekeeper { get; set; }
    public Material? Material { get; set; }
}
