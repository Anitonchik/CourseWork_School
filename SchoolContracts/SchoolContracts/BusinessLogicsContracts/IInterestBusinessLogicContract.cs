using SchoolContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.BusinessLogicsContracts;

public interface IInterestBusinessLogicContract
{
    List<InterestDataModel> GetAllInterests();
    List<InterestDataModel> GetAllInterestsByWorker(string workerId);
    InterestDataModel GetInterestByData(string data);
    List<InterestMaterialDataModel> GetMaterialsByInterestId(string interestId);
    void InsertInterest(InterestDataModel interestDataModel);
    void UpdateInterest(InterestDataModel interestDataModel);
    void DeleteInterest(string id);
}
