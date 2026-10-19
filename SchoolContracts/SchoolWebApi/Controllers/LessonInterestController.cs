using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolContracts.AdapterContracts;
using SchoolContracts.BindingModels;

namespace SchoolWebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class LessonInterestController(ILessonInterestAdapter adapter) : ControllerBase
{
    private readonly ILessonInterestAdapter _adapter = adapter;

    /*[HttpPost]
    public IActionResult Register(string workerId, [FromBody] LessonBindingModel lessonModel, [FromBody] LessonInterestBindingModel lessonInterestModel)
    {
        return _adapter.RegisterLessonInterest(workerId, lessonModel, lessonInterestModel).GetResponse(Request, Response);
    }*/

    [HttpDelete]
    public IActionResult Delete(string workerId, string lessonId, string interestId)
    {
        return _adapter.RemoveLessonInterest(workerId, lessonId, interestId).GetResponse(Request, Response);
    }
}
