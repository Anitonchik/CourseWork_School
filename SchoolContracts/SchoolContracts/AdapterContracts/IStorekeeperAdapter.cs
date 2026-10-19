using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;

namespace SchoolContracts.AdapterContracts;

public interface IStorekeeperAdapter
{
    StorekeeperOperationResponse GetUserByLogin(string login);

    /*StorekeeperOperationResponse RegisterStorekeeper(StorekeeperBindingModel storekeeperModel);

    StorekeeperOperationResponse ChangeStorekeeperInfo(StorekeeperBindingModel storekeeperModel);

    StorekeeperOperationResponse RemoveStorekeeper(string id);*/
}
