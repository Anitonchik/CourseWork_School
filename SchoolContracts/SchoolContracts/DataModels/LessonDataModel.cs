using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.DataModels;

public class LessonDataModel(string id, string workerId, string achievementId, string lessonName, string description)
{
    public string Id { get; private set; } = id;
    public string WorkerId { get; private set; } = workerId;
    public string AchievementId { get; private set; } = achievementId;
    public string LessonName { get; private set; } = lessonName;
    
    public DateTime LessonDate { get; private set; } = DateTime.UtcNow;
    public string Description { get; private set; } = description;
}
