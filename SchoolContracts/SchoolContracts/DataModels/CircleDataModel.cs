using SchoolContracts.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SchoolContracts.DataModels;

public class CircleDataModel
{
    public string Id { get; private set; }
    public string StorekeeperId { get; private set; }
    public string CircleName { get; private set; }
    public string Description { get; private set; }
    public List<CircleMaterialDataModel> Materials { get; private set; }
    public List<LessonCircleDataModel> Lessons { get; private set; }

    public CircleDataModel() { }
 
    public CircleDataModel(string id, string storekeeperId, string circleName,
        string description, List<CircleMaterialDataModel> circleMaterials, List<LessonCircleDataModel> lessonCircles)
    {
        Id = id;
        StorekeeperId = storekeeperId;
        CircleName = circleName;
        Description = description;
        Materials = circleMaterials;
        Lessons = lessonCircles;
    }

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
