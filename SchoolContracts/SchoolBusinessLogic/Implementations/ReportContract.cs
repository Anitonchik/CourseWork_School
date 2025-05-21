using Microsoft.Extensions.Logging;
using SchoolBuisnessLogic.OfficePackage;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.ModelsForReports;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models.ModelsForReports;

namespace SchoolBuisnessLogic.Implementations;

internal class ReportContract(ICircleStorageContract circleStorageContract, IInterestStorageContract interestStorageContract,
    ILessonStorageContract lessonStorageContract,  IMaterialStorageContract materialStorageContract,
    BaseWordBuilder baseWordBuilder, BaseExcelBuilder baseExcelBuilder, ILogger logger) : IReportContract
{
    private readonly ILessonStorageContract _lessonStorageContract = lessonStorageContract;
    private readonly ICircleStorageContract _circleStorageContract = circleStorageContract;
    private readonly IInterestStorageContract _interestStorageContract = interestStorageContract;
    private readonly IMaterialStorageContract _materialStorageContract = materialStorageContract;
    private readonly BaseWordBuilder _baseWordBuilder = baseWordBuilder;
    private readonly BaseExcelBuilder _baseExcelBuilder = baseExcelBuilder;
    private readonly ILogger _logger = logger;

    internal static readonly string[] docHeaderLessonByMaterial = ["Название материала", "Название занятия", "Описание занятия"];
    internal static readonly string[] docHeaderMaterialByLesson = ["Название занятия", "Название материала", "Количество материала"];
    internal static readonly string[] docHeaderCirclesWithInterestsWithMedals = ["Название кружка", "Описание кружка", "Название интереса", "Название медальки", "Дата"];
    internal static readonly string[] docHeaderInterestsWithAchievementsWithCircles = ["Название интереса", "Описание интереса", "Название кружка", "Название достижения", "Дата"];

    public async Task<Stream> CreateDocumentCirclesWithInterestsWithMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        _logger.LogInformation("Create report CirclesWithInterestsWithMedals from {dateStart} to {dateFinish}", fromDate, toDate);
        var data = await GetCirclesWithInterestsWithMedals(storekeeperId, fromDate, toDate, ct);
        return _baseWordBuilder
            .AddHeader("Список кружков")
            .AddParagraph($"С {fromDate} по {toDate}")
            .AddTable([5000, 5000, 5000, 5000, 3000], [.. new List<string[]>() { docHeaderCirclesWithInterestsWithMedals }
            .Union([.. data.SelectMany(x => (new List<string[]>() { new string[] { x.CircleName, x.CircleDescription, x.InterestName, x.MedalName, x.Date.ToShortDateString() } }))])])
            .Build();
    }

    public async Task<Stream> CreateDocumentInterestsWithAchievementsWithCircles(string workerId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        _logger.LogInformation("Create report InterestsWithAchievementsWithCircles from {dateStart} to {dateFinish}", fromDate, toDate);
        var data = await GetInterestsWithAchievementsWithCircles(workerId, fromDate, toDate, ct);
        return _baseWordBuilder
            .AddHeader("Список интересов")
            .AddParagraph($"С {fromDate} по {toDate}")
            .AddTable([5000, 5000, 5000, 5000, 3000], [.. new List<string[]>() { docHeaderInterestsWithAchievementsWithCircles }
            .Union([.. data.SelectMany(x => (new List<string[]>() { new string[] { x.InterestName, x.InterestDescription, x.CircleName, x.AchievementName, x.Date.ToShortDateString() } }))])])
            .Build();
    }

    public async Task<Stream> CreateWordDocumentLessonByMaterialsAsync(string storekeeperId, List<string> materialIds, CancellationToken ct)
    {
        _logger.LogInformation("Create word report LessonByMaterials");
        var data = await GetLessonsByMaterial(storekeeperId, materialIds, ct);
        return _baseWordBuilder
            .AddHeader("Список занятий по материалам")
            .AddTable([3000, 5000, 5000], [.. new List<string[]>() { docHeaderLessonByMaterial   }
            .Union([.. data.SelectMany(x => (new List<string[]>() { new string[] { x.Key, "", "" } })
            .Union(x.Value.Select(y => new string[] { "", y.LessonName, y.LessonDescription })))])])
            .Build();
    }

    public async Task<Stream> CreateExcelDocumentLessonByMaterialsAsync(string storekeeperId, List<string> materialIds, CancellationToken ct)
    {
        _logger.LogInformation("Create excel report LessonByMaterials");
        var data = await GetLessonsByMaterial(storekeeperId, materialIds, ct) ?? throw new InvalidOperationException("No found data");
        return _baseExcelBuilder
            .AddHeader("Список занятий по материалам", 0, 3)
            .AddTable([10, 10, 10], [.. new List<string[]>() { docHeaderLessonByMaterial }
            .Union(data.SelectMany(x => (new List<string[]>()
            { new string[] { x.Key, "", "" } })
            .Union(x.Value!
             .Select(y => new string[] { "", y.LessonName, y.LessonDescription })).ToArray()))])
            .Build();
    }

    public async Task<Stream> CreateWordDocumentMaterialByLessonsAsync(string workerId, List<string> lessonIds, CancellationToken ct)
    {
        _logger.LogInformation("Create report LessonByMaterials");
        var data = await GetMaterialsByLesson(workerId, lessonIds, ct);
        return _baseWordBuilder
            .AddHeader("Список материалов по занятиям")
            .AddTable([3000, 5000, 3000], [.. new List<string[]>() { docHeaderMaterialByLesson }
            .Union([.. data.SelectMany(x => (new List<string[]>() { new string[] { x.Key, "", "" } })
            .Union(x.Value.Select(y => new string[] { "", y.MaterialName, y.Count.ToString()})))])])
            .Build();
    }

    public async Task<Stream> CreateExcelDocumentMaterialByLessonsAsync(string workerId, List<string> lessonIds, CancellationToken ct)
    {
        _logger.LogInformation("Create excel report LessonByMaterials");
        var data = await GetMaterialsByLesson(workerId, lessonIds, ct) ?? throw new InvalidOperationException("No found data");
        return _baseExcelBuilder
            .AddHeader("Список материалов по занятиям", 0, 3)
            .AddTable([10, 10, 10], [.. new List<string[]>() { docHeaderMaterialByLesson }
            .Union(data.SelectMany(x => (new List<string[]>()
            { new string[] { x.Key, "", "" } })
            .Union(x.Value!
             .Select(y => new string[] { "", y.MaterialName, y.Count.ToString()})).ToArray()))])
            .Build();
    }


    // storekeeper
    public Task<List<CirclesWithInterestsWithMedalsModel>> GetCirclesWithInterestsWithMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        _logger.LogInformation("Get data CirclesWithInterestsWithMedals from {dateStart} to {dateFinish}", fromDate, toDate);
        return _circleStorageContract.GetCirclesWithInterestsWithMedals(storekeeperId, fromDate, toDate, ct);
    }
    
    // worker
    public Task<List<InterestsWithAchievementsWithCirclesModel>> GetInterestsWithAchievementsWithCircles(string workerId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        _logger.LogInformation("Get data InterestsWithAchievementsWithCircles from {dateStart} to {dateFinish}", fromDate, toDate);
        return _interestStorageContract.GetInterestsWithAchievementsWithCircles(workerId, fromDate, toDate, ct);
    }

    public async Task<Dictionary<string, List<LessonByMaterialModel>>> GetLessonsByMaterial(string storekeeperId, List<string> materialIds, CancellationToken ct)
    {
        _logger.LogInformation("Get data LessonsByMaterial");
        Dictionary<string, List<LessonByMaterialModel>> lessonsByMaterial = new Dictionary<string, List<LessonByMaterialModel>>();

        foreach (var materialId in materialIds)
        {
            var lessons = await _lessonStorageContract.GetLessonsByMaterial(storekeeperId, materialId, ct);
            lessonsByMaterial.Add(lessons.First().MaterialName, lessons);
        }
        return lessonsByMaterial;
    }

    public async Task<Dictionary<string, List<MaterialByLessonModel>>> GetMaterialsByLesson(string workerId, List<string> lessonIds, CancellationToken ct)
    {
        _logger.LogInformation("Get data MaterialsByLesson");
        Dictionary<string, List<MaterialByLessonModel>> materialByLessons = new Dictionary<string, List<MaterialByLessonModel>>();

        foreach (var lessonId in lessonIds)
        {
            var materials = await _materialStorageContract.GetMaterialsByLesson(workerId, lessonId, ct);
            materialByLessons.Add(materials.First().LessonName, materials);
        }
        return materialByLessons;
    }
}
