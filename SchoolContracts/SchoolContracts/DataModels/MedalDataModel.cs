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
}
