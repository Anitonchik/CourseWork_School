using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolContracts.AdapterContracts;
using SchoolContracts.BindingModels;

namespace SchoolWebApi.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class LessonsController(ILessonAdapter adapter) : ControllerBase
{
    private readonly ILessonAdapter _adapter = adapter;

    [HttpGet("GetWholeRecords")]
    public IActionResult GetWholeRecords()
    {
        return _adapter.GetWholeList().GetResponse(Request, Response);
    }

    [HttpGet]
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
    public IActionResult Register( [FromBody] LessonBindingModel model)
    {
        return _adapter.RegisterLesson(model.WorkerId, model).GetResponse(Request, Response);
    }

    [HttpPut("ChangeInfo")]
    public IActionResult ChangeInfo( [FromBody] LessonBindingModel model)
    {
        return _adapter.ChangeLessonInfo(model.WorkerId, model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string workerId, string id)
    {
        return _adapter.RemoveLesson(workerId, id).GetResponse(Request, Response);
    }
}
