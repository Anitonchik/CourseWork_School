using SchoolContracts.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SchoolContracts.DataModels;

public class LessonCircleDataModel
{
    public LessonDataModel? _lesson;
    public string LessonId { get; private set; }
    public string CircleId { get; private set; }
    public int Count { get; private set; }

    public string LessonName => _lesson?.LessonName ?? string.Empty;

    public LessonCircleDataModel() { }

    public LessonCircleDataModel(string lessonId, string circleId, int count)
    {
        LessonId = lessonId;
        CircleId = circleId;    
        Count = count;
    }

    public LessonCircleDataModel(string lessonId, string circleId, int count, LessonDataModel lesson) : this(lessonId, circleId, count)
    {
        _lesson = lesson;
    }

    public void Validate()
    {
        if (LessonId.IsEmpty())
            throw new ValidationException("Field LessonId is empty");
        if (!LessonId.IsGuid())
            throw new ValidationException("The value in the field OrderId is not a unique identifier");
        if (CircleId.IsEmpty())
            throw new ValidationException("Field CircleId is empty");
        if (!CircleId.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");
        if (Count <= 0)
            throw new ValidationException("Field Count is less than or equal to 0");
    }
}
