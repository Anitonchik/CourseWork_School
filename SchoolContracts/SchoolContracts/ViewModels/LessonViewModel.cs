using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.ViewModels;

public class LessonViewModel
{
    public required string Id { get; set; }
    public required string WorkerId { get; set; }
    public required string LessonName { get; set; }
    public string Description { get; set; }
}
