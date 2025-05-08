using SchoolContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.BusinessLogicsContracts;

internal interface IAchievementBuisnessLogicContract
{
    List<AchievementDataModel> GetAllAchievementsByPeriod(DateTime fromDate, DateTime toDate);
    List<AchievementDataModel> GetAllAchievementsByWorker(string workerId);
    AchievementDataModel GetAchievementByData(string data);
    void InsertAchievement(AchievementDataModel achievementDataModel);
    void UpdateAchievement(AchievementDataModel achievementDataModel);
    void DeleteAchievement(string id);
}
