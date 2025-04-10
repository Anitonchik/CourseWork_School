namespace SchoolContracts.DataModels;

public class CircleMaterialDataModel (string circleId, string materialId, int count)
{
    public string CircleId { get; private set; } = circleId;
    public string MaterialId { get; private set; } = materialId;
    public int Count { get; private set; } = count;
}
