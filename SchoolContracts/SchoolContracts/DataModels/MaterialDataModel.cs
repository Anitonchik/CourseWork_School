using SchoolContracts.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SchoolContracts.DataModels;
public class MaterialDataModel 
{
    public string Id { get; private set; }
    public string StorekeeperId { get; private set; }
    public string MaterialName { get; private set; }
    public string Description { get; private set; }

    public MaterialDataModel() { }

    public MaterialDataModel(string id, string storekeeperId, string materialName, string description)
    {
        Id = id;
        StorekeeperId = storekeeperId;
        MaterialName = materialName;
        Description = description;
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

        if (MaterialName.IsEmpty())
            throw new ValidationException("Field MaterialName is empty");
    }
}
