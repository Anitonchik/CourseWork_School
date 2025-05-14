using SchoolContracts.Infrastructure;
using SchoolContracts.ModelsForReports;
using SchoolContracts.ViewModels;

namespace SchoolContracts.AdapterContracts.OperationResponses;

public class ReportOperationResponse : OperationResponse
{
    public static ReportOperationResponse OK(List<CirclesWithInterestsWithMedalsViewModel> data) => OK<ReportOperationResponse, List<CirclesWithInterestsWithMedalsViewModel>>(data);

    public static ReportOperationResponse OK(List<InterestsWithAchievementsWithCirclesViewModel> data) => OK<ReportOperationResponse, List<InterestsWithAchievementsWithCirclesViewModel>>(data);

    public static ReportOperationResponse OK(List<MaterialByLessonViewModel> data) => OK<ReportOperationResponse, List<MaterialByLessonViewModel>>(data);

    public static ReportOperationResponse OK(List<LessonByMaterialViewModel> data) => OK<ReportOperationResponse, List<LessonByMaterialViewModel>>(data);

    public static ReportOperationResponse OK(Stream data, string fileName) => OK<ReportOperationResponse, Stream>(data, fileName);

    public static ReportOperationResponse BadRequest(string message) => BadRequest<ReportOperationResponse>(message);

    public static ReportOperationResponse InternalServerError(string message) => InternalServerError<ReportOperationResponse>(message);
}
