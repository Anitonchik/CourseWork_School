namespace SchoolContracts.DataModels;

public class CircleDataModel (string id, string storekeeperId, string circleName, string description)
{
    public string Id { get; private set; } = id;
    public string StorekeeperId { get; private set; } = storekeeperId;
    public string CircleName { get; private set; } = circleName;
    public string Description { get; private set; } = description;
}
