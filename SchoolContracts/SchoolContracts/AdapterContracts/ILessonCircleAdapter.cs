using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;

namespace SchoolContracts.AdapterContracts;

public interface ILessonCircleAdapter
{

    LessonCircleOperationResponse RegisterLessonCircle(string storekeeperId, string circleId, string lessonId, int count);

    LessonCircleOperationResponse RemoveLessonCircle(string storekeeperId, string lessonId, string circleId);
}
