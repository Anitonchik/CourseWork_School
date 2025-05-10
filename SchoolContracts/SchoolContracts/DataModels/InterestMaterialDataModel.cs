using SchoolContracts.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SchoolContracts.DataModels;

public class InterestMaterialDataModel (string interesId, string materialId)
{
    public string InterestId { get; private set; } = interesId;
    public string MaterialId { get; private set; } = materialId;

    public void Validate()
    {
        if (InterestId.IsEmpty())
            throw new ValidationException("Field InterestId is empty");
        if (!InterestId.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");
        if (MaterialId.IsEmpty())
            throw new ValidationException("Field MaterialId is empty");
        if (!MaterialId.IsGuid())
            throw new ValidationException("The value in the field OrderId is not a unique identifier");
    }
}
