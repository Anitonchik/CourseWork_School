using SchoolContracts.DataModels;

namespace SchoolContracts.StoragesContracts;

public interface IMedalStorageContract
{
    List<MedalDataModel> GetList();
    MedalDataModel? GetElementById(string id);
    MedalDataModel? GetElementByName(string name);
    void AddElement(MedalDataModel medalDataModel);
    void UpdElement(MedalDataModel medalDataModel);
    void DelElement(string id);
}
