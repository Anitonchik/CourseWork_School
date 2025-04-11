namespace SchoolContracts.DataModels;

public class CircleDataModel (string id, string storekeeperId, string circleName, 
    string description, List<CircleMaterialDataModel> materials, List<LessonCircleDataModel> lessons)
{
    public string Id { get; private set; } = id;
    public string StorekeeperId { get; private set; } = storekeeperId;
    public string CircleName { get; private set; } = circleName;
    public string Description { get; private set; } = description;
    public List<CircleMaterialDataModel> Materials { get; private set; } = materials;
    public List<LessonCircleDataModel> Lessons { get; private set; } = lessons;
}
