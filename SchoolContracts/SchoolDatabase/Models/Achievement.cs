using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDatabase.Models;

public class Achievement
{
    public required string Id { get; set; } = Guid.NewGuid().ToString();
    public required string WorkerId { get; set; }
    public required string AchievementName { get; set; }
    public DateTime AchievementDate { get; set; }
    public string Description { get; set; }
    public Worker? Worker { get; set; }
}
