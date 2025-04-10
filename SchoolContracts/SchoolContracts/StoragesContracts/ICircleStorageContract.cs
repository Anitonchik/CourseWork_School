using SchoolContracts.DataModels;

namespace SchoolContracts.StoragesContracts;

public interface ICircleStorageContract
{
    List<CircleDataModel> GetList();
    CircleDataModel? GetElementById(string id);
    CircleDataModel? GetElementByName(string name);
    void AddElement(CircleDataModel circleDataModel);
    void UpdElement(CircleDataModel circleDataModel);
    void DelElement(string id);
}
