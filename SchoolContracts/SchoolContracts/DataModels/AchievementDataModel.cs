using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.DataModels;

public class AchievementDataModel(string id, string workerId, string achievementName, string description)
{
    public string Id { get; private set; } = id;
    public string WorkerId { get; private set; } = workerId;
    public string AchievementName { get; private set; } = achievementName;
    public DateTime AchievementDate { get; private set; } = DateTime.UtcNow;
    public string Description { get; private set; } = description;
}
