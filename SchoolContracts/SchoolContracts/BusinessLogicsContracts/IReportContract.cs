using SchoolContracts.ModelsForReports;
using SchoolDatabase.Models.ModelsForReports;

namespace SchoolContracts.BusinessLogicsContracts;

public interface IReportContract
{
    Task<List<CirclesWithInterestsWithMedalsModel>> GetCirclesWithInterestsWithMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct);
    Task<List<InterestsWithAchievementsWithCirclesModel>> GetInterestsWithAchievementsWithCircles(string workerId, DateTime fromDate, DateTime toDate, CancellationToken ct);
    Task<List<MaterialByLesson>> GetMaterialsByLesson(string storekeeperId, string lessonId, CancellationToken ct);
    Task<List<LessonByMaterialModel>> GetLessonsByMaterial(string workerId, string materialId, CancellationToken ct);

    Task<Stream> CreateDocumentCirclesWithInterestsWithMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct);
}
