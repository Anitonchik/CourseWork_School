using SchoolContracts.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SchoolContracts.DataModels;
public class MaterialDataModel (string id, string storekeeperId, string materialName, string description)
{
    public string Id { get; private set; } = id;
    public string StorekeeperId { get; private set; } = storekeeperId;
    public string MaterialName { get; private set; } = materialName;
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

        if (MaterialName.IsEmpty())
            throw new ValidationException("Field MaterialName is empty");
    }
}
