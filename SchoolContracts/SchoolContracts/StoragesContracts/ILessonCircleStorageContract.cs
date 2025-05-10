using SchoolContracts.DataModels;

namespace SchoolContracts.StoragesContracts;

public interface ILessonCircleStorageContract
{
    public void AddElement(LessonCircleDataModel lessonCircleDataModel);
    public void DeleteElement(string lessonId, string circleId);
}
