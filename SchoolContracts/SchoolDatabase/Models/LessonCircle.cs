namespace SchoolDatabase.Models;

public class LessonCircle
{
    public required string LessonId { get; set; }
    public required string CircleId { get; set; }
    public int Count { get; set; }

    public Lesson? Lesson { get; set; }
    public Circle? Circle { get; set; }
}
