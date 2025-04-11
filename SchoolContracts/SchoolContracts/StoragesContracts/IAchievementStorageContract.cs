using SchoolContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.StoragesContracts;

public interface IAchievementStorageContract
{
    List<AchievementDataModel> GetList();
    AchievementDataModel? GetElementById(string id);
    AchievementDataModel? GetElementByName(string name);
    void AddElement(AchievementDataModel achievementDataModel);
    void UpdElement(AchievementDataModel achievementDataModel);
    void DelElement(string id);
}
