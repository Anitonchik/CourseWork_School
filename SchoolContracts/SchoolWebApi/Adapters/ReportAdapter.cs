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

    public async Task<ReportOperationResponse> CreateDocumentCirclesWithInterestsWithMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        try
        {
            return await SendStream(await _reportContract.CreateDocumentCirclesWithInterestsWithMedals(storekeeperId, fromDate, toDate, ct), "CirclesWithInterestsWithMedals.docx");
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

    private async Task<ReportOperationResponse> SendStream(object value, string v)
    {
        throw new NotImplementedException();
    }

    public async Task<ReportOperationResponse> GetCirclesWithInterestsWithMedalsAsync(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        try
        {
            return ReportOperationResponse.OK([.. (await _reportContract.GetCirclesWithInterestsWithMedals(storekeeperId, fromDate, toDate, ct)).Select(x => _mapper.Map<CirclesWithInterestsWithMedalsViewModel>(x))]);
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

    public Task<ReportOperationResponse> GetInterestsWithAchievementsWithCirclesAsync(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<ReportOperationResponse> GetLessonsByMaterialAsync(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<ReportOperationResponse> GetMaterialsByLessonAsync(string workerId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
