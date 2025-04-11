using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.DataModels;

public class LessonInterestDataModel(string lessonId, string interesId)
{
    public string LessonId { get; private set; } = lessonId;
    public string InterestId { get; private set; } = interesId;
    
}
