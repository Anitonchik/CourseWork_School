using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;

namespace SchoolContracts.AdapterContracts;

public interface ICircleAdapter
{
    CircleOperationResponse GetList(string storekeeperId);

    CircleOperationResponse GetElement(string storekeeperId, string data);

    CircleOperationResponse RegisterCircle(string storekeeperId, CircleBindingModel circleModel);

    CircleOperationResponse ChangeCircleInfo(string storekeeperId, CircleBindingModel circleModel);

    CircleOperationResponse RemoveCircle(string storekeeperId, string id);
}
