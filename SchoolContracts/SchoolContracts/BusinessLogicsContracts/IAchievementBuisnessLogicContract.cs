using SchoolContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.BusinessLogicsContracts;
public interface IAchievementBuisnessLogicContract
{
    List<AchievementDataModel> GetAllAchievements(string workerId);
    AchievementDataModel GetAchievementById(string workerId, string id);
    //void CreateConnectWithLesson(string workerId, string achievementId, string lessonId);
    void InsertAchievement(string workerId,AchievementDataModel achievementDataModel);
    void UpdateAchievement(string workerId,AchievementDataModel achievementDataModel);
    void DeleteAchievement(string workerId,string id);
}
