using SchoolContracts.DataModels;

namespace SchoolContracts.StoragesContracts;

public interface IStorekeeperStorageContract
{
    List<StorekeeperDataModel> GetList();
    StorekeeperDataModel? GetElementById(string id);
    StorekeeperDataModel? GetElementByFIO(string fio);
    StorekeeperDataModel? GetElementByLogin(string login);
    StorekeeperDataModel? GetElementByMail(string mail);
    void AddElement(StorekeeperDataModel storekeeperDataModel);
    void UpdElement(StorekeeperDataModel storekeeperDataModel);
    void DelElement(string id);
}
