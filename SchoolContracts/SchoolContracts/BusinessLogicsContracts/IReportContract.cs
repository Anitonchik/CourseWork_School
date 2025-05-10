using SchoolContracts.ModelsForReports;
using SchoolDatabase.Models.ModelsForReports;

namespace SchoolContracts.BusinessLogicsContracts;

public interface IReportContract
{
    List<CirclesWithInterestsWithMedals> GetCirclesWithInterestsWithMedals(string storekeeperId, DateTime fromDate, DateTime toDate);
    List<InterestsWithAchievementsWithCircles> GetInterestsWithAchievementsWithCircles(string workerId, DateTime fromDate, DateTime toDate);
    List<MaterialByLesson> GetMaterialsByLesson(string storekeeperId, string lessonId);
    List<LessonByMaterial> GetLessonsByMaterial(string workerId, string materialId);
}
