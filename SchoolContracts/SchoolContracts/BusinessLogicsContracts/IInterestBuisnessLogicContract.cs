using SchoolContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.BusinessLogicsContracts;

public interface IInterestBuisnessLogicContract
{
    List<InterestDataModel> GetAllInterests(string workerId);
    InterestDataModel GetInterestByData(string workerId,string data);
    //List<InterestMaterialDataModel> GetMaterialsByInterestId(string interestId);
    void InsertInterest(string workerId, InterestDataModel interestDataModel);
    void UpdateInterest(string workerId, InterestDataModel interestDataModel);
    void DeleteInterest(string workerId, string id);
}
