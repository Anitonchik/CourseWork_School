using SchoolContracts.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SchoolContracts.DataModels;

public class CircleMaterialDataModel (string circleId, string materialId, int count)
{
    public string CircleId { get; private set; } = circleId;
    public string MaterialId { get; private set; } = materialId;
    public int Count { get; private set; } = count;

    public void Validate()
    {
        if (MaterialId.IsEmpty())
            throw new ValidationException("Field MaterialId is empty");
        if (!MaterialId.IsGuid())
            throw new ValidationException("The value in the field OrderId is not a unique identifier");
        if (CircleId.IsEmpty())
            throw new ValidationException("Field CircleId is empty");
        if (!CircleId.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");
        if (Count <= 0)
            throw new ValidationException("Field Count is less than or equal to 0");
    }
}
