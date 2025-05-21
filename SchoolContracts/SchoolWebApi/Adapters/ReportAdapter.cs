using AutoMapper;
using SchoolContracts.AdapterContracts;
using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.Exceptions;
using SchoolContracts.ModelsForReports;
using SchoolContracts.ViewModels;

namespace SchoolWebApi.Adapters;

public class ReportAdapter : IReportAdapter
{
    private readonly IReportContract _reportContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public ReportAdapter(IReportContract reportContract, ILogger<ReportAdapter> logger)
    {
        _reportContract = reportContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CirclesWithInterestsWithMedalsModel, CirclesWithInterestsWithMedalsViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public async Task<ReportOperationResponse> CreateDocumentCirclesWithInterestsWithMedalsAsync(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        try
        {
            return SendStream(await _reportContract.CreateDocumentCirclesWithInterestsWithMedals(storekeeperId, fromDate, toDate, ct), "CirclesWithInterestsWithMedals.docx");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> CreateDocumentInterestsWithAchievementsWithCirclesAsync(string workerId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        try
        {
            return SendStream(await _reportContract.CreateDocumentInterestsWithAchievementsWithCircles(workerId, fromDate, toDate, ct), "InterestsWithAchievementsWithCircles.docx");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> CreateWordDocumentLessonByMaterialsAsync(string storekeeperId, List<string> materialIds, CancellationToken ct)
    {
        try
        {
            return SendStream(await _reportContract.CreateWordDocumentLessonByMaterialsAsync(storekeeperId, materialIds, ct), "LessonByMaterials.docx");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }
    public async Task<ReportOperationResponse> CreateWordDocumentMaterialByLessonsAsync(string workerId, List<string> lessonIds, CancellationToken ct)
    {
        try
        {
            return SendStream(await _reportContract.CreateWordDocumentMaterialByLessonsAsync(workerId, lessonIds, ct), "MaterialByLessons.docx");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> CreateExcelDocumentLessonByMaterialsAsync(string storekeeperId, List<string> materialIds, CancellationToken ct)
    {
        try
        {
            return SendStream(await _reportContract.CreateExcelDocumentLessonByMaterialsAsync(storekeeperId, materialIds, ct), "LessonByMaterials.xslx");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> CreateExcelDocumentMaterialByLessonsAsync(string workerId, List<string> lessonIds, CancellationToken ct)
    {
        try
        {
            return SendStream(await _reportContract.CreateExcelDocumentMaterialByLessonsAsync(workerId, lessonIds, ct), "MaterialByLessons.xslx");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> GetCirclesWithInterestsWithMedalsAsync(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        try
        {
            return ReportOperationResponse.OK([.. (await _reportContract.GetCirclesWithInterestsWithMedals(storekeeperId, fromDate, toDate, ct)).Select(x => _mapper.Map<CirclesWithInterestsWithMedalsViewModel>(x))]);
        }
        catch (IncorrectDatesException ex)
        {
            _logger.LogError(ex, "IncorrectDatesException");
            return ReportOperationResponse.BadRequest($"Incorrect dates: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> GetInterestsWithAchievementsWithCirclesAsync(string workerId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        try
        {
            return ReportOperationResponse.OK((await _reportContract.GetInterestsWithAchievementsWithCircles(workerId, fromDate, toDate, ct)).Select(x => _mapper.Map<InterestsWithAchievementsWithCirclesViewModel>(x)).ToList());
        }
        catch (IncorrectDatesException ex)
        {
            _logger.LogError(ex, "IncorrectDatesException");
            return ReportOperationResponse.BadRequest($"Incorrect dates: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> GetLessonsByMaterialAsync(string storekeeperId, List<string> materialIds, CancellationToken ct)
    {
        try
        {
            return ReportOperationResponse.OK((await _reportContract.GetLessonsByMaterial(storekeeperId, materialIds, ct)).Select(x => _mapper.Map<LessonByMaterialViewModel>(x.Value)).ToList());
        }
        catch (IncorrectDatesException ex)
        {
            _logger.LogError(ex, "IncorrectDatesException");
            return ReportOperationResponse.BadRequest($"Incorrect dates: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> GetMaterialsByLessonAsync(string workerId, List<string> lessonIds, CancellationToken ct)
    {
        try
        {
            return ReportOperationResponse.OK((await _reportContract.GetMaterialsByLesson(workerId, lessonIds, ct)).Select(x => _mapper.Map<MaterialByLessonViewModel>(x.Value)).ToList());
        }
        catch (IncorrectDatesException ex)
        {
            _logger.LogError(ex, "IncorrectDatesException");
            return ReportOperationResponse.BadRequest($"Incorrect dates: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    private static ReportOperationResponse SendStream(Stream stream, string fileName)
    {
        stream.Position = 0;
        return ReportOperationResponse.OK(stream, fileName);
    }
}
