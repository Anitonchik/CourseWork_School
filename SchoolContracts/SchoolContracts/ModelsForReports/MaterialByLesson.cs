namespace SchoolDatabase.Models.ModelsForReports;

public class MaterialByLesson
{
    public string LessonName { get; set; }
    public string MaterialName { get; set; }
    public string LessonDescription { get; set; }
    public int? Count { get; set; }
}
