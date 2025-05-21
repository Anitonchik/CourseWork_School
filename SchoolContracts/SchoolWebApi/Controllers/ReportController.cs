using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolContracts.AdapterContracts;
using SchoolContracts.ModelsForReports;
using SchoolDatabase.Models;
using SchoolDatabase.Models.ModelsForReports;

namespace SchoolWebApi.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class ReportController(IReportAdapter adapter) : ControllerBase
{
    private readonly IReportAdapter _adapter = adapter;

    [HttpGet]
    [Consumes("application/json")]
    public async Task<IActionResult> GetCirclesWithInterestsWidthMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        return (await _adapter.GetCirclesWithInterestsWithMedalsAsync(storekeeperId, fromDate, toDate, ct)).GetResponse(Request, Response);
    }

    [HttpGet]
    [Consumes("application/json")]
    public async Task<IActionResult> GetInterestsWithAchievementsWithCircles(string workerId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        return (await _adapter.GetInterestsWithAchievementsWithCirclesAsync(workerId, fromDate, toDate, ct)).GetResponse(Request, Response);
    }

    [HttpGet]
    [Consumes("application/json")]
    public async Task<IActionResult> GetLessonsByMaterial(string storekeeperId, List<string> materialIds, CancellationToken ct)
    {
        return (await _adapter.GetLessonsByMaterialAsync(storekeeperId, materialIds, ct)).GetResponse(Request, Response);
    }

    [HttpGet]
    [Consumes("application/json")]
    public async Task<IActionResult> GetMaterialsByLesson(string workerId, List<string> lessonIds, CancellationToken ct)
    {
        return (await _adapter.GetMaterialsByLessonAsync(workerId, lessonIds, ct)).GetResponse(Request, Response);
    }

    [HttpGet]
    [Consumes("application/octet-stream")]
    public async Task<IActionResult> LoadCirclesInterestsMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        return (await _adapter.CreateDocumentCirclesWithInterestsWithMedalsAsync(storekeeperId, fromDate, toDate, ct)).GetResponse(Request, Response);
    }

    [HttpGet]
    [Consumes("application/octet-stream")]
    public async Task<IActionResult> CreateDocumentCirclesWithInterestsWithMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        return (await _adapter.CreateDocumentCirclesWithInterestsWithMedalsAsync(storekeeperId, fromDate, toDate, ct)).GetResponse(Request, Response);
    }

    [HttpGet]
    [Consumes("application/octet-stream")]
    public async Task<IActionResult> CreateDocumentInterestsWithAchievementsWithCircles(string workerId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        return (await _adapter.CreateDocumentInterestsWithAchievementsWithCirclesAsync(workerId, fromDate, toDate, ct)).GetResponse(Request, Response);
    }

    [HttpGet]
    [Consumes("application/octet-stream")]
    public async Task<IActionResult> CreateWordDocumentLessonByMaterialsAsync(string storekeeperId, List<string> materialIds, CancellationToken ct)
    {
        return (await _adapter.CreateWordDocumentLessonByMaterialsAsync(storekeeperId, materialIds, ct)).GetResponse(Request, Response);
    }

    [HttpGet]
    [Consumes("application/octet-stream")]
    public async Task<IActionResult> CreateExcelDocumentLessonByMaterialsAsync(string storekeeperId, List<string> materialIds, CancellationToken ct)
    {
        return (await _adapter.CreateExcelDocumentLessonByMaterialsAsync(storekeeperId, materialIds, ct)).GetResponse(Request, Response);
    }

    [HttpGet]
    [Consumes("application/octet-stream")]
    public async Task<IActionResult> CreateWordDocumentMaterialByLessonsAsync(string workerId, List<string> lessonIds, CancellationToken ct)
    {
        return (await _adapter.CreateWordDocumentMaterialByLessonsAsync(workerId, lessonIds, ct)).GetResponse(Request, Response);
    }

    [HttpGet]
    [Consumes("application/octet-stream")]
    public async Task<IActionResult> CreateExcelDocumentMaterialByLessonsAsync(string workerId, List<string> lessonIds, CancellationToken ct)
    {
        return (await _adapter.CreateExcelDocumentMaterialByLessonsAsync(workerId, lessonIds, ct)).GetResponse(Request, Response);
    }
}
