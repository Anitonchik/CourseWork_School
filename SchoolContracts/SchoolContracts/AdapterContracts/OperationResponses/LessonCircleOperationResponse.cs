using SchoolContracts.Infrastructure;
using SchoolContracts.ViewModels;

namespace SchoolContracts.AdapterContracts.OperationResponses;

public class LessonCircleOperationResponse : OperationResponse
{
    public static LessonCircleOperationResponse OK(List<LessonCircleViewModel> data) => OK<LessonCircleOperationResponse, List<LessonCircleViewModel>>(data);

    public static LessonCircleOperationResponse OK(LessonCircleViewModel data) => OK<LessonCircleOperationResponse, LessonCircleViewModel>(data);

    public static LessonCircleOperationResponse NoContent() => NoContent<LessonCircleOperationResponse>();

    public static LessonCircleOperationResponse BadRequest(string message) => BadRequest<LessonCircleOperationResponse>(message);

    public static LessonCircleOperationResponse NotFound(string message) => NotFound<LessonCircleOperationResponse>(message);

    public static LessonCircleOperationResponse InternalServerError(string message) => InternalServerError<LessonCircleOperationResponse>(message);
}
