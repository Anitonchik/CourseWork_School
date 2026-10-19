using SchoolContracts.DataModels;

namespace SchoolContracts.BusinessLogicsContracts;

public interface IStorekeeperBuisnessLogicContract
{
    StorekeeperDataModel GetStorekeeperByLogin(string login);
    StorekeeperDataModel GetStorekeeperByMail(string mail);
    void InsertStorekeeper(StorekeeperDataModel storekeeperDataModel);
    void UpdateStorekeeper(StorekeeperDataModel storekeeperDataModel);
    void DeleteStorekeeper(string id);
}
