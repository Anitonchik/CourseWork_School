using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolContracts.AdapterContracts;
using SchoolContracts.BindingModels;
using SchoolDatabase.Models;

namespace SchoolWebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class AchievementsController(IAchievementAdapter adapter) : ControllerBase
{
    private readonly IAchievementAdapter _adapter = adapter;

    [HttpGet("GetAllRecords")]
    public IActionResult GetAllRecords(string workerId)
    {
        return _adapter.GetList(workerId).GetResponse(Request, Response);
    }

    [HttpGet("{data}")]
    public IActionResult GetRecord(string workerId, string data)
    {
        return _adapter.GetElement(workerId, data).GetResponse(Request, Response);
    }

    [HttpPost("register")]
    public IActionResult Register( [FromBody] AchievementBindingModel model)
    {
        return _adapter.RegisterAchievement(model.WorkerId, model).GetResponse(Request, Response);
    }

    [HttpPut("ChangeInfo")]
    public IActionResult ChangeInfo( [FromBody] AchievementBindingModel model)
    {
        return _adapter.ChangeAchievementInfo(model.WorkerId, model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string workerId, string id)
    {
        return _adapter.RemoveAchievement(workerId, id).GetResponse(Request, Response);
    }
}
