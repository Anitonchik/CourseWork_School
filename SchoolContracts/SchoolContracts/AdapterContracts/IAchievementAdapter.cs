using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.AdapterContracts;

public interface IAchievementAdapter
{
    AchievementOperationResponse GetList(string workerId);

    AchievementOperationResponse GetElement(string workerId, string id);

    AchievementOperationResponse RegisterAchievement(string workerId, AchievementBindingModel achievementModel);

    AchievementOperationResponse ChangeAchievementInfo(string workerId, AchievementBindingModel achievementModel);

    AchievementOperationResponse RemoveAchievement(string workerId, string id);
}
