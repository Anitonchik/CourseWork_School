using SchoolContracts.Infrastructure;
using SchoolContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.AdapterContracts.OperationResponses;

public class LessonInterestOperationResponse : OperationResponse
{
    public static LessonInterestOperationResponse OK(List<LessonInterestViewModel> data) => OK<LessonInterestOperationResponse, List<LessonInterestViewModel>>(data);

    public static LessonInterestOperationResponse OK(LessonInterestViewModel data) => OK<LessonInterestOperationResponse, LessonInterestViewModel>(data);

    public static LessonInterestOperationResponse NoContent() => NoContent<LessonInterestOperationResponse>();

    public static LessonInterestOperationResponse BadRequest(string message) => BadRequest<LessonInterestOperationResponse>(message);

    public static LessonInterestOperationResponse NotFound(string message) => NotFound<LessonInterestOperationResponse>(message);

    public static LessonInterestOperationResponse InternalServerError(string message) => InternalServerError<LessonInterestOperationResponse>(message);
}
