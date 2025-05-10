using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;

namespace SchoolContracts.AdapterContracts;

public interface IStorekeeperAdapter
{
    StorekeeperOperationResponse RegisterStorekeeper(string storekeeperId, StorekeeperBindingModel storekeeperModel);

    StorekeeperOperationResponse ChangeStorekeeperInfo(string storekeeperId, StorekeeperBindingModel storekeeperModel);

    StorekeeperOperationResponse RemoveStorekeeper(string storekeeperId, string id);
}
