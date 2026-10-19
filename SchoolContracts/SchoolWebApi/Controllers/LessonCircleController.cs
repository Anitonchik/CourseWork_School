using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolContracts.AdapterContracts;
using SchoolContracts.BindingModels;

namespace SchoolWebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class LessonCircleController(ILessonCircleAdapter adapter) : ControllerBase
{
    private readonly ILessonCircleAdapter _adapter = adapter;

    [HttpPost("register")]
    public IActionResult Register([FromBody] LessonCircleBindingModel lessonCircle)
    {
        return _adapter.RegisterLessonCircle(lessonCircle.UserId, lessonCircle.LessonId, lessonCircle.CircleId, lessonCircle.Count).GetResponse(Request, Response);
    }

    [HttpDelete]
    public IActionResult Delete(string storekeeperId, string lessonId, string circleId)
    {
        return _adapter.RemoveLessonCircle(storekeeperId, lessonId, circleId).GetResponse(Request, Response);
    }
}
