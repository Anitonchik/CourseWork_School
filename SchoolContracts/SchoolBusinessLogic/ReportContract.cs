using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.ModelsForReports;
using SchoolDatabase.Models.ModelsForReports;

namespace SchoolBuisnessLogic;

public class ReportContract : IReportContract
{
    public Task<List<CirclesWithInterestsWithMedalsModel>> GetCirclesWithInterestsWithMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<List<InterestsWithAchievementsWithCirclesModel>> GetInterestsWithAchievementsWithCircles(string workerId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<List<LessonByMaterialModel>> GetLessonsByMaterial(string workerId, string materialId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<List<MaterialByLesson>> GetMaterialsByLesson(string storekeeperId, string lessonId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
