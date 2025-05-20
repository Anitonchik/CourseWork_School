using SchoolContracts.ModelsForReports;
using SchoolDatabase.Models.ModelsForReports;

namespace SchoolContracts.BusinessLogicsContracts;

public interface IReportContract
{
    Task<List<CirclesWithInterestsWithMedalsModel>> GetCirclesWithInterestsWithMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct);
    Task<List<InterestsWithAchievementsWithCirclesModel>> GetInterestsWithAchievementsWithCircles(string workerId, DateTime fromDate, DateTime toDate, CancellationToken ct);
    Task<Dictionary<string, List<LessonByMaterialModel>>> GetLessonsByMaterial(string storekeeperId, List<string> materialIds, CancellationToken ct);
    Task<Dictionary<string, List<MaterialByLessonModel>>> GetMaterialsByLesson(string workerId, List<string> lessonIds, CancellationToken ct);
    Task<Stream> CreateDocumentCirclesWithInterestsWithMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct);
    Task<Stream> CreateDocumentInterestsWithAchievementsWithCircles(string workerId, DateTime fromDate, DateTime toDate, CancellationToken ct);
    Task<Stream> CreateWordDocumentLessonByMaterialsAsync(string storekeeperId, List<string> materialIds, CancellationToken ct);
    Task<Stream> CreateExcelDocumentLessonByMaterialsAsync(string storekeeperId, List<string> materialIds, CancellationToken ct);
    Task<Stream> CreateWordDocumentMaterialByLessonsAsync(string workerId, List<string> lessonIds, CancellationToken ct);
    Task<Stream> CreateExcelDocumentMaterialByLessonsAsync(string workerId, List<string> lessonIds, CancellationToken ct)
}
