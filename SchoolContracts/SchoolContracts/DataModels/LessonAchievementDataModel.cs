using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.DataModels;

public class LessonAchievementDataModel(string lessonId, string achievementId)
{
    public string LessonId { get; private set; } = lessonId;
    public string AchievementId { get; private set; } = achievementId;
}
