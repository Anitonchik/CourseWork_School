using SchoolContracts.AdapterContracts.OperationResponses;

namespace SchoolContracts.AdapterContracts;

public interface IReportAdapter
{
    Task<ReportOperationResponse> GetCirclesWithInterestsWithMedalsAsync(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct);
    Task<ReportOperationResponse> GetInterestsWithAchievementsWithCirclesAsync(string workerId, DateTime fromDate, DateTime toDate, CancellationToken ct);
    Task<ReportOperationResponse> GetLessonsByMaterialAsync(string storekeeperId, List<string> materialIds, CancellationToken ct);
    Task<ReportOperationResponse> GetMaterialsByLessonAsync(string workerId, List<string> lessonIds, CancellationToken ct);
    Task<ReportOperationResponse> CreateDocumentCirclesWithInterestsWithMedalsAsync(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct);
    Task<ReportOperationResponse> CreateDocumentInterestsWithAchievementsWithCirclesAsync(string workerId, DateTime fromDate, DateTime toDate, CancellationToken ct);
    Task<ReportOperationResponse> CreateWordDocumentLessonByMaterialsAsync(string storekeeperId, List<string> materialIds, CancellationToken ct);
    Task<ReportOperationResponse> CreateExcelDocumentLessonByMaterialsAsync(string storekeeperId, List<string> materialIds, CancellationToken ct);
    Task<ReportOperationResponse> CreateWordDocumentMaterialByLessonsAsync(string workerId, List<string> lessonIds, CancellationToken ct);
    Task<ReportOperationResponse> CreateExcelDocumentMaterialByLessonsAsync(string workerId, List<string> lessonIds, CancellationToken ct);
}
