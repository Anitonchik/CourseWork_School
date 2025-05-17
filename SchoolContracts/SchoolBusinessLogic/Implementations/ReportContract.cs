using Microsoft.Extensions.Logging;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.ModelsForReports;
using SchoolContracts.StoragesContracts;
using SchoolDatabase;
using SchoolDatabase.Models.ModelsForReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBuisnessLogic.Implementations;

internal class ReportContract : IReportContract
{
    private readonly ILessonStorageContract _lessonStorage;
    private readonly IMaterialStorageContract _materialStorage;
    private readonly ILogger _logger;

    public ReportContract(ILessonStorageContract lessonStorage, IMaterialStorageContract materialStorage, ILogger logger)
    {
        _lessonStorage = lessonStorage;
        _materialStorage = materialStorage;
        _logger = logger;
    }
    public List<CirclesWithInterestsWithMedals> GetCirclesWithInterestsWithMedals(string storekeeperId, DateTime fromDate, DateTime toDate)
    {
        throw new NotImplementedException();
    }

    public List<InterestsWithAchievementsWithCircles> GetInterestsWithAchievementsWithCircles(string workerId, DateTime fromDate, DateTime toDate)
    {
        throw new NotImplementedException();
    }

    public List<LessonByMaterial> GetLessonsByMaterial(string workerId, string materialId)
    {
        throw new NotImplementedException();
    }

    public List<MaterialByLesson> GetMaterialsByLesson(string storekeeperId, string lessonId)
    {
        throw new NotImplementedException();
    }
}
