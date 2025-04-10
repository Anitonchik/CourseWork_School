using SchoolContracts.DataModels;

namespace SchoolContracts.StoragesContracts;

public interface IMedalStorageContract
{
    List<MaterialDataModel> GetList();
    MaterialDataModel? GetElementById(string id);
    MaterialDataModel? GetElementByName(string name);
    void AddElement(MaterialDataModel materialDataModel);
    void UpdElement(MaterialDataModel materialDataModel);
    void DelElement(string id);
}
