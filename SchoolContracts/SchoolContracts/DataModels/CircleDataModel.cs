namespace SchoolContracts.DataModels;

public class CircleDataModel (string Id, string StorekeeperId, string CircleName, 
    string Description, List<CircleMaterialDataModel> CircleMaterials, List<LessonCircleDataModel> LessonCircles)
{
    public string Id { get; private set; } = Id;
    public string StorekeeperId { get; private set; } = StorekeeperId;
    public string CircleName { get; private set; } = CircleName;
    public string Description { get; private set; } = Description;
    public List<CircleMaterialDataModel> Materials { get; private set; } = CircleMaterials;
    public List<LessonCircleDataModel> Lessons { get; private set; } = LessonCircles;
}
