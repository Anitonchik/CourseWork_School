namespace SchoolContracts.DataModels;

public class InterestMaterialDataModel (string interesId, string materialId)
{
    public string InterestId { get; private set; } = interesId;
    public string MaterialId { get; private set; } = materialId;
}
