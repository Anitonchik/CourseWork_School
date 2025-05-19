using SchoolContracts.AdapterContracts.OperationResponses;

namespace SchoolContracts.AdapterContracts;

public interface IReportAdapter
{
    Task<ReportOperationResponse> GetCirclesWithInterestsWithMedalsAsync(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct);
    Task<ReportOperationResponse> GetInterestsWithAchievementsWithCirclesAsync(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct);
    Task<ReportOperationResponse> GetMaterialsByLessonAsync(string workerId, DateTime fromDate, DateTime toDate, CancellationToken ct);
    Task<ReportOperationResponse> GetLessonsByMaterialAsync(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct);

    Task<ReportOperationResponse> CreateDocumentCirclesWithInterestsWithMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct);

}
