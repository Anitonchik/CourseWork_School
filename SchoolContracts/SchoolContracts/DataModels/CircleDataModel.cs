using SchoolContracts.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SchoolContracts.DataModels;

public class CircleDataModel(string id, string storekeeperId, string circleName,
    string description, List<CircleMaterialDataModel> circleMaterials, List<LessonCircleDataModel> lessonCircles)
{
    public string Id { get; private set; } = id;
    public string StorekeeperId { get; private set; } = storekeeperId;
    public string CircleName { get; private set; } = circleName;
    public string Description { get; private set; } = description;
    public List<CircleMaterialDataModel> Materials { get; private set; } = circleMaterials;
    public List<LessonCircleDataModel> Lessons { get; private set; } = lessonCircles;

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");

        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");

        if (StorekeeperId.IsEmpty())
            throw new ValidationException("Field StorekeeperId is empty");

        if (!StorekeeperId.IsGuid())
            throw new ValidationException("The value in the field StorekeeperId is not a unique identifier");

        if (CircleName.IsEmpty())
            throw new ValidationException("Field CircleName is empty");
    }
}
