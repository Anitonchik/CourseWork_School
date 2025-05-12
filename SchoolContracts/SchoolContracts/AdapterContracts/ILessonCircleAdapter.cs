using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;

namespace SchoolContracts.AdapterContracts;

public interface ILessonCircleAdapter
{

    LessonCircleOperationResponse RegisterLessonCircle(string storekeeperId, CircleBindingModel circleModel, LessonCircleBindingModel LessonCircleModel);

    LessonCircleOperationResponse RemoveLessonCircle(string storekeeperId, string lessonId, string circleId);
}
