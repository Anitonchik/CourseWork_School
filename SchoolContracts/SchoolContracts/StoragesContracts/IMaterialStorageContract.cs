using SchoolContracts.DataModels;
using SchoolDatabase.Models.ModelsForReports;


namespace SchoolContracts.StoragesContracts;

public interface IMaterialStorageContract
{
    List<MaterialDataModel> GetList(string storekeeperId);
    List<MaterialByLesson> GetMaterialsByLesson(string storekeeperId, string lessonId);
    MaterialDataModel? GetElementById(string id);
    MaterialDataModel? GetElementByName(string name);
    void AddElement(MaterialDataModel materialDataModel);
    void UpdElement(MaterialDataModel materialDataModel);
    void DelElement(string id);
}
