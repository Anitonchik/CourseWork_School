using SchoolContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.StoragesContracts;

public interface IAchievementStorageContract
{
    List<AchievementDataModel> GetList(string workerId);
    AchievementDataModel? GetElementById(string id);
    //void CreateConnectWithLesson(string achievementId, string lessonId);
    void AddElement(AchievementDataModel achievementDataModel);
    void UpdElement(AchievementDataModel achievementDataModel);
    void DelElement(string id);
}
