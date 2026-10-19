using SchoolContracts.Infrastructure;
using SchoolContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.AdapterContracts.OperationResponses;

public class LessonOperationResponse : OperationResponse
{
    public static LessonOperationResponse OK(List<LessonViewModel> data) => OK<LessonOperationResponse, List<LessonViewModel>>(data);

    public static LessonOperationResponse OK(LessonViewModel data) => OK<LessonOperationResponse, LessonViewModel>(data);

    public static LessonOperationResponse NoContent() => NoContent<LessonOperationResponse>();

    public static LessonOperationResponse BadRequest(string message) => BadRequest<LessonOperationResponse>(message);

    public static LessonOperationResponse NotFound(string message) => NotFound<LessonOperationResponse>(message);

    public static LessonOperationResponse InternalServerError(string message) => InternalServerError<LessonOperationResponse>(message);
}
