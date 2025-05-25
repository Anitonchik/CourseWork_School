using SchoolContracts.DataModels;

namespace SchoolContracts.StoragesContracts;

public interface ILessonCircleStorageContract
{
    public void AddElement(LessonCircleDataModel lessonCircleDataModel);
    public LessonCircleDataModel? GetLessonCircleById(string lessonId, string circleId);
}
