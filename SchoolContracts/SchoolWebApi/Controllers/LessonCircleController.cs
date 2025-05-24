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

    [HttpPost]
    public IActionResult Register(string storekeeperId, string circleId, string lessonId, int count)
    {
        return _adapter.RegisterLessonCircle(storekeeperId, circleId, lessonId, count).GetResponse(Request, Response);
    }

    [HttpDelete]
    public IActionResult Delete(string storekeeperId, string lessonId, string circleId)
    {
        return _adapter.RemoveLessonCircle(storekeeperId, lessonId, circleId).GetResponse(Request, Response);
    }
}
