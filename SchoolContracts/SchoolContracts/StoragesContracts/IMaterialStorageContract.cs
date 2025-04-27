using SchoolContracts.DataModels;

namespace SchoolContracts.StoragesContracts;

public interface IMaterialStorageContract
{
    List<MaterialDataModel> GetList();
    List<MaterialDataModel> GetMaterialsByLesson(string lessonId);
    MaterialDataModel? GetElementById(string id);
    MaterialDataModel? GetElementByName(string name);
    void AddElement(MaterialDataModel materialDataModel);
    void UpdElement(MaterialDataModel materialDataModel);
    void DelElement(string id);
}
