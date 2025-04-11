using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDatabase.Models;

public class Interest
{
    public required string Id { get; set; } = Guid.NewGuid().ToString();
    public required string WorkerId { get; set; }
    public required string InterestName { get; set; }
    public string Description { get; set; }
    public Worker? Worker { get; set; }

    [ForeignKey("InterestId")]
    public List<CircleMaterial>? CircleMaterials { get; set; }

    [ForeignKey("InterestId")]
    public List<InterestMaterial>? InterestMaterials { get; set; }
}
