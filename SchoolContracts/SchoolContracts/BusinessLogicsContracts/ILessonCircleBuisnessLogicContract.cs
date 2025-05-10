using SchoolContracts.DataModels;

namespace SchoolContracts.BusinessLogicsContracts;

public interface ILessonCircleBuisnessLogicContract
{
    public void CreateLessonCircle(string storekeeperId, LessonCircleDataModel lessonCircleDataModel);
    public void DeleteLessonCircle(string storekeeperId, string lessonId, string circleId);
}
