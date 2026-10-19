using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.ViewModels;

public class AchievementViewModel
{
    public required string Id { get; set; }
    public required string WorkerId { get; set; }
    public required string LessonId { get; set; }
    public required string AchievementName { get; set; }
    public string Description { get; set; }
}
