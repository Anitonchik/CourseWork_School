using SchoolContracts.DataModels;

namespace SchoolContracts.BusinessLogicsContracts;

public interface IStorekeeperBuisnessLogicContract
{
    void InsertStorekeeper(StorekeeperDataModel storekeeperDataModel);
    void UpdateStorekeeper(StorekeeperDataModel storekeeperDataModel);
    void DeleteStorekeeper(string id);
}
