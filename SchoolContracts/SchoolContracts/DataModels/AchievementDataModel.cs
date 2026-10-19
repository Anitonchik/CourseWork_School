using SchoolContracts.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SchoolContracts.DataModels;

public class AchievementDataModel
{
    public string Id { get; private set; }
    public string WorkerId { get; private set; } 
    public string? LessonId { get; private set; } 
    public string AchievementName { get; private set; } 
    //public DateTime AchievementDate { get; private set; } = DateTime.UtcNow;
    public string Description { get; private set; }
    public AchievementDataModel() { }
    public AchievementDataModel(string id, string workerId, string? lessonId, string achievementName/*, DateTime achievementDate*/,
    string description)
    {
        Id = id;
        WorkerId = workerId;
        LessonId = lessonId;
        AchievementName = achievementName;
        //AchievementDate = achievementDate;
        Description = description;
    }
    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");

        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");

        if (WorkerId.IsEmpty())
            throw new ValidationException("Field WorkerId is empty");

        if (!WorkerId.IsGuid())
            throw new ValidationException("The value in the field WorkerId is not a unique identifier");

        if (AchievementName.IsEmpty())
            throw new ValidationException("Field AchievementName is empty");
    }
}
