using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolContracts.AdapterContracts;

namespace SchoolWebApi.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class ReportController(IReportAdapter adapter) : ControllerBase
{
    private readonly IReportAdapter _adapter = adapter;

    [HttpGet]
    [Consumes("application/json")]
    public async Task<IActionResult> GetCirclesInterestsMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        return (await _adapter.GetCirclesWithInterestsWithMedalsAsync(storekeeperId, fromDate, toDate, ct)).GetResponse(Request, Response);
    }

    [HttpGet]
    [Consumes("application/octet-stream")]
    public async Task<IActionResult> LoadCirclesInterestsMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        return (await _adapter.CreateDocumentCirclesWithInterestsWithMedals(storekeeperId, fromDate, toDate, ct)).GetResponse(Request, Response);
    }
}
