using SchoolContracts.DataModels;

namespace SchoolContracts.StoragesContracts;

public interface IConnectEntitiesStorageContract
{
    public void CreateConnectLessonAndCircle (string lessonId, string circleId, int count);
    public void DeleteConnectLessonAndCircle(string lessonId, string circleId);

    public void CreateConnectLessonAndInterest(string lessonId, string interestId);
    public void DeleteConnectLessonAndInterest(string lessonId, string interestId);

}
