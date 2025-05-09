using SchoolContracts.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SchoolContracts.DataModels;

public class MedalDataModel(string id, string storekeeperId, string? materialId, string medalName,
    int range, string description)
{
    public string Id { get; private set; } = id;
    public string StorekeeperId { get; private set; } = storekeeperId;
    public string? MaterialId { get; private set; } = materialId;
    public string MedalName { get; private set; } = medalName;
    public int Range { get; private set; } = range;
    public string Description { get; private set; } = description;

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

        if (MedalName.IsEmpty())
            throw new ValidationException("Field MedalName is empty");

        if (Range <= 0)
            throw new ValidationException("Field Range is less than or equal to 0");
    }
}
