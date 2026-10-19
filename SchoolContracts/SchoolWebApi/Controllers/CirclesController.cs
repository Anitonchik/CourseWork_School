using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SchoolContracts.AdapterContracts;
using SchoolContracts.BindingModels;

namespace SchoolWebApi.Controllers;


[Authorize]
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class CirclesController(ICircleAdapter adapter) : ControllerBase
{
    private readonly ICircleAdapter _adapter = adapter;

    [HttpGet("GetAllRecords")]
    public IActionResult GetAllRecords(string storekeeperId)
    {
        return _adapter.GetList(storekeeperId).GetResponse(Request, Response);
    }

    [HttpGet("{data}")]
    public IActionResult GetRecord(string storekeeperId, string data)
    {
        return _adapter.GetElement(storekeeperId, data).GetResponse(Request, Response);
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] CircleBindingModel model)
    {
        return _adapter.RegisterCircle(model.StorekeeperId, model).GetResponse(Request, Response);
    }

    [HttpPut("ChangeInfo")]
    public IActionResult ChangeInfo([FromBody] CircleBindingModel model)
    {
        return _adapter.ChangeCircleInfo(model.StorekeeperId, model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string storekeeperId, string id)
    {
        return _adapter.RemoveCircle(storekeeperId, id).GetResponse(Request, Response);
    }
}
