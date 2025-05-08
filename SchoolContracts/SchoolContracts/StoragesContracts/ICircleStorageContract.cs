using SchoolContracts.DataModels;
using SchoolContracts.ModelsForReports;

namespace SchoolContracts.StoragesContracts;

public interface ICircleStorageContract
{
    List<CircleDataModel> GetList(string storekeeperId);
    List<CirclesWithInterestsWithMedals> GetCirclesWithInterestsWithMedals(string storekeeperId, DateTime fromDate, DateTime toDate);
    CircleDataModel? GetElementById(string id);
    CircleDataModel? GetElementByName(string name);
    void AddElement(CircleDataModel circleDataModel);
    void UpdElement(CircleDataModel circleDataModel);
    void DelElement(string id);
}
