using SchoolContracts.BindingModels;

namespace SchoolContracts.ViewModels;

public class CircleViewModel
{
    public required string Id { get; set; }
    public required string StorekeeperId { get; set; }
    public required string CircleName { get; set; }
    public string Description { get; set; }
    public List<CircleMaterialViewModel>? Materials { get; set; }

    public List<LessonCircleViewModel>? Lessons { get; set; }
}
