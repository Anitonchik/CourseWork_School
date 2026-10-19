using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;

namespace SchoolContracts.AdapterContracts;

public interface IMedalAdapter
{
    MedalOperationResponse GetList(string storekeeperId);

    MedalOperationResponse GetListByRange(string storekeeperId, int range);

    MedalOperationResponse GetElement(string storekeeperId, string id);

    MedalOperationResponse RegisterMedal(string storekeeperId, MedalBindingModel medalModel);

    MedalOperationResponse ChangeMedalInfo(string storekeeperId, MedalBindingModel medalModel);

    MedalOperationResponse RemoveMedal(string storekeeperId, string id);
}
