using SchoolContracts.DataModels;

namespace SchoolContracts.StoragesContracts;

public interface IConnectEntitiesStorageContract
{
    public void CreateConnectLessonAndCircle (string lessonId, string circleId);
    public void DeleteConnectLessonAndCircle(string lessonId, string circleId);
    public void CreateConnectCircleAndMaterial (string circleId, string materialId, int count = 1);
    public void DeleteConnectCircleAndMaterial (string circleId, string materialId, int count = 1);

    public void CreateConnectLessonAndInterest(string lessonId, string interestId);
    public void DeleteConnectLessonAndInterest(string lessonId, string interestId);
    public void CreateConnectInterestAndMaterial(string interestId, string materialId);
    public void DeleteConnectInterestAndMaterial(string interestId, string materialId);

}
