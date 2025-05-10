using SchoolContracts.Infrastructure;
using SchoolContracts.ViewModels;

namespace SchoolContracts.AdapterContracts.OperationResponses;

public class MedalOperationResponse : OperationResponse
{
    public static MedalOperationResponse OK(List<MedalViewModel> data) => OK<MedalOperationResponse, List<MedalViewModel>>(data);

    public static MedalOperationResponse OK(MedalViewModel data) => OK<MedalOperationResponse, MedalViewModel>(data);

    public static MedalOperationResponse NoContent() => NoContent<MedalOperationResponse>();

    public static MedalOperationResponse BadRequest(string message) => BadRequest<MedalOperationResponse>(message);

    public static MedalOperationResponse NotFound(string message) => NotFound<MedalOperationResponse>(message);

    public static MedalOperationResponse InternalServerError(string message) => InternalServerError<MedalOperationResponse>(message);
}
