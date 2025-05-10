using SchoolContracts.DataModels;
using SchoolContracts.ModelsForReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.StoragesContracts;

public interface IInterestStorageContract
{
    List<InterestDataModel> GetList(string workerId);
   // List<InterestsWithAchievementsWithCircles> GetInterestsWithAchievementsWithCircles(string workerId,DateTime startDate, DateTime endDate);
    InterestDataModel? GetElementById(string id);
    InterestDataModel? GetElementByName(string name);
    void AddElement(InterestDataModel interestDataModel);
    void UpdElement(InterestDataModel interestDataModel);
    void DelElement(string id);
}
