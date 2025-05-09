using SchoolContracts.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SchoolContracts.DataModels;

public class AchievementDataModel(string id, string workerId, string achievementName, string description)
{
    public string Id { get; private set; } = id;
    public string WorkerId { get; private set; } = workerId;
    public string AchievementName { get; private set; } = achievementName;
    public DateTime AchievementDate { get; private set; } = DateTime.UtcNow;
    public string Description { get; private set; } = description;

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
