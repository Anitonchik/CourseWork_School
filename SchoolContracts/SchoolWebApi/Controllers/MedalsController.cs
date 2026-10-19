using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolContracts.AdapterContracts;
using SchoolContracts.BindingModels;

namespace SchoolWebApi.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
[Produces("application/json")]
public class MedalsController(IMedalAdapter adapter) : ControllerBase
{
    private readonly IMedalAdapter _adapter = adapter;

    [HttpGet("GetAllRecords")]
    public IActionResult GetAllRecords(string storekeeperId)
    {
        return _adapter.GetList(storekeeperId).GetResponse(Request, Response);
    }
    
    [HttpGet]
    public IActionResult GetRecordsByRange(string storekeeperId, int range)
    {
        return _adapter.GetListByRange(storekeeperId, range).GetResponse(Request, Response);
    }

    [HttpGet("{data}")]
    public IActionResult GetRecord(string storekeeperId, string data)
    {
        return _adapter.GetElement(storekeeperId, data).GetResponse(Request, Response);
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] MedalBindingModel model)
    {
        return _adapter.RegisterMedal(model.StorekeeperId, model).GetResponse(Request, Response);
    }

    [HttpPut("ChangeInfo")]
    public IActionResult ChangeInfo([FromBody] MedalBindingModel model)
    {
        return _adapter.ChangeMedalInfo(model.StorekeeperId, model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string storekeeperId, string id)
    {
        return _adapter.RemoveMedal(storekeeperId, id).GetResponse(Request, Response);
    }
}
