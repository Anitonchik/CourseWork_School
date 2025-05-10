using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.BindingModels;

public class LessonBindingModel
{
    public string? Id { get; set; }
    public string? WorkerId { get; set; }
    public string LessonName { get; set; }
    public string Description { get; set; }
}
