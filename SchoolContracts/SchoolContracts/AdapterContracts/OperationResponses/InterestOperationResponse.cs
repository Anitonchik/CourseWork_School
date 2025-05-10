using SchoolContracts.Infrastructure;
using SchoolContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.AdapterContracts.OperationResponses;

public class InterestOperationResponse : OperationResponse
{
    public static InterestOperationResponse OK(List<InterestViewModel> data) => OK<InterestOperationResponse, List<InterestViewModel>>(data);

    public static InterestOperationResponse OK(InterestViewModel data) => OK<InterestOperationResponse, InterestViewModel>(data);

    public static InterestOperationResponse NoContent() => NoContent<InterestOperationResponse>();

    public static InterestOperationResponse BadRequest(string message) => BadRequest<InterestOperationResponse>(message);

    public static InterestOperationResponse NotFound(string message) => NotFound<InterestOperationResponse>(message);

    public static InterestOperationResponse InternalServerError(string message) => InternalServerError<InterestOperationResponse>(message);
}
