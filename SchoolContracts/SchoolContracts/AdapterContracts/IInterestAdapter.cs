using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.AdapterContracts;

public interface  IInterestAdapter
{
    InterestOperationResponse GetList(string workerId);

    InterestOperationResponse GetElement(string workerId, string data);

    InterestOperationResponse RegisterInterest(string workerId, InterestBindingModel interestModel);

    InterestOperationResponse ChangeInterestInfo(string workerId, InterestBindingModel interestModel);

    InterestOperationResponse RemoveInterest(string workerId, string id);
}
