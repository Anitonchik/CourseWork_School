using SchoolContracts.Infrastructure;
using SchoolContracts.ViewModels;

namespace SchoolContracts.AdapterContracts.OperationResponses;

public class CircleOperationResponse : OperationResponse
{
    public static CircleOperationResponse OK(List<CircleViewModel> data) => OK<CircleOperationResponse, List<CircleViewModel>>(data);

    public static CircleOperationResponse OK(CircleViewModel data) => OK<CircleOperationResponse, CircleViewModel>(data);

    public static CircleOperationResponse NoContent() => NoContent<CircleOperationResponse>();

    public static CircleOperationResponse BadRequest(string message) => BadRequest<CircleOperationResponse>(message);

    public static CircleOperationResponse NotFound(string message) => NotFound<CircleOperationResponse>(message);

    public static CircleOperationResponse InternalServerError(string message) => InternalServerError<CircleOperationResponse>(message);
}
