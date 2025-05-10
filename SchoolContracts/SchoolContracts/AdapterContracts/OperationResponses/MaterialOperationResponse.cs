using SchoolContracts.Infrastructure;
using SchoolContracts.ViewModels;

namespace SchoolContracts.AdapterContracts.OperationResponses;

public class MaterialOperationResponse : OperationResponse
{
    public static MaterialOperationResponse OK(List<MaterialViewModel> data) => OK<MaterialOperationResponse, List<MaterialViewModel>>(data);

    public static MaterialOperationResponse OK(MaterialViewModel data) => OK<MaterialOperationResponse, MaterialViewModel>(data);

    public static MaterialOperationResponse NoContent() => NoContent<MaterialOperationResponse>();

    public static MaterialOperationResponse BadRequest(string message) => BadRequest<MaterialOperationResponse>(message);

    public static MaterialOperationResponse NotFound(string message) => NotFound<MaterialOperationResponse>(message);

    public static MaterialOperationResponse InternalServerError(string message) => InternalServerError<MaterialOperationResponse>(message);
}
