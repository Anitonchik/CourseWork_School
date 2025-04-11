using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDatabase.Models;

public class LessonInterest
{
    public required string LessonId { get; set; }
    public required string InterestId { get; set; }
    public Lesson? Lesson { get; set; }
    public Interest? Interest { get; set; }   
}
