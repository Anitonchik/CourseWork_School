using SchoolContracts.DataModels;

namespace SchoolContracts.StoragesContracts;

public interface ILessonCircleStorageContract
{
    public LessonCircleDataModel? GetLessonCircleById(string lessonId, string circleId);
}
