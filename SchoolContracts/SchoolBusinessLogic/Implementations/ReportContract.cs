using Microsoft.Extensions.Logging;
using SchoolBuisnessLogic.OfficePackage;
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

internal class ReportContract(ICircleStorageContract circleStorageContract, ILessonStorageContract lessonStorageContract, 
    BaseWordBuilder baseWordBuilder, ILogger logger) : IReportContract
{
    private readonly ILessonStorageContract _lessonStorageContract = lessonStorageContract;
    private readonly ICircleStorageContract _circleStorageContract = circleStorageContract;
    private readonly BaseWordBuilder _baseWordBuilder = baseWordBuilder;
    private readonly ILogger _logger = logger;

    public async Task<Stream> CreateDocumentCirclesWithInterestsWithMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        _logger.LogInformation("Create report PricesByProducts");
        var data = await GetCirclesWithInterestsWithMedals(storekeeperId, fromDate, toDate, ct);
        return _baseWordBuilder
            .AddHeader("История цен по продуктам")
            .AddParagraph($"Сформировано на дату {DateTime.Now}")
            .AddTable([3000, 5000], [.. new List<string[]>() { documentHeader }
            .Union([.. data.SelectMany(x => (new List<string[]>() { new string[] { x.ProductName, "" } })
            .Union(x.Prices.Select(y => new string[] { "", y })))])])
            .Build();
    }
    
    public async Task<Stream> CreateDocumentLessonByMaterials(string storekeeperId, List<string> materialIds, CancellationToken ct)
    {
        _logger.LogInformation("Create report LessonByMaterials");
        var data = await GetCirclesWithInterestsWithMedals(storekeeperId, fromDate, toDate, ct);
        return _baseWordBuilder
            .AddHeader("История цен по продуктам")
            .AddParagraph($"Сформировано на дату {DateTime.Now}")
            .AddTable([3000, 5000], [.. new List<string[]>() { documentHeader }
            .Union([.. data.SelectMany(x => (new List<string[]>() { new string[] { x.ProductName, "" } })
            .Union(x.Prices.Select(y => new string[] { "", y })))])])
            .Build();
    }


    // storekeeper
    public Task<List<CirclesWithInterestsWithMedalsModel>> GetCirclesWithInterestsWithMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        _logger.LogInformation("Get data CirclesWithInterestsWithMedals from {dateStart} to {dateFinish}", fromDate, toDate);
        return _circleStorageContract.GetCirclesWithInterestsWithMedals(storekeeperId, fromDate, toDate, ct);
    }
    
    public async Task<List<LessonByMaterialModel>> GetLessonsByMaterial(string storekeeperId, List<string> materialIds, CancellationToken ct)
    {
        _logger.LogInformation("Get data LessonsByMaterial");
        Dictionary<string, LessonByMaterialModel> circle = new Dictionary<string, LessonByMaterialModel>();

        foreach (var materialId in materialIds)
        {
            circle.Add(materialId, await _lessonStorageContract.GetLessonsByMaterial(storekeeperId, materialId, ct));
        }
        return _lessonStorageContract.GetLessonsByMaterial(storekeeperId, materialId, ct);
    }

    // worker
    public Task<List<InterestsWithAchievementsWithCirclesModel>> GetInterestsWithAchievementsWithCircles(string workerId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<List<MaterialByLesson>> GetMaterialsByLesson(string workerId, string lessonId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
