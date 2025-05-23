using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolContracts.AdapterContracts;
using SchoolContracts.BindingModels;

namespace SchoolWebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class InterestsController(IInterestAdapter adapter) : ControllerBase
{
    private readonly IInterestAdapter _adapter = adapter;

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
    public IActionResult Register([FromBody] InterestBindingModel model)
    {
        return _adapter.RegisterInterest(model.WorkerId, model).GetResponse(Request, Response);
    }

    [HttpPut("ChangeInfo")]
    public IActionResult ChangeInfo( [FromBody] InterestBindingModel model)
    {
        return _adapter.ChangeInterestInfo(model.WorkerId, model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string workerId, string id)
    {
        return _adapter.RemoveInterest(workerId, id).GetResponse(Request, Response);
    }
}
