using SchoolContracts.DataModels;

namespace SchoolContracts.BusinessLogicsContracts;

public interface ICircleBuisnessLogicContract
{
    List<CircleDataModel> GetAllCircles(string storekeeperId);
    CircleDataModel GetCircleByData(string storekeeperId, string data);
    void InsertCircle(string storekeeperId, CircleDataModel circleDataModel);
    void UpdateCircle(string storekeeperId, CircleDataModel circleDataModel);
    void DeleteCircle(string storekeeperId, string id);
}
