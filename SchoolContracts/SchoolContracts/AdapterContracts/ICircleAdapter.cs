using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;

namespace SchoolContracts.AdapterContracts;

public interface ICircleAdapter
{
    CircleOperationResponse GetList();

    CircleOperationResponse GetElement(string data);

    CircleOperationResponse RegisterCircle(CircleBindingModel circleModel);

    CircleOperationResponse ChangeCircleInfo(CircleBindingModel circleModel);

    CircleOperationResponse RemoveCircle(string id);
}
