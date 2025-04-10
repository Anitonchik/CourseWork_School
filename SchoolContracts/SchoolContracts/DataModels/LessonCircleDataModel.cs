namespace SchoolContracts.DataModels;

public class LessonCircleDataModel (string lessonId, string circleId)
{
    public string LessonId { get; private set; } = lessonId;
    public string CircleId { get; private set; } = circleId;
}
