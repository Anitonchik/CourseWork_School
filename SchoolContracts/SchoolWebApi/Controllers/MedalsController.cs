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

    [HttpGet]
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

    [HttpPost]
    public IActionResult Register(string storekeeperId, [FromBody] MedalBindingModel model)
    {
        return _adapter.RegisterMedal(storekeeperId, model).GetResponse(Request, Response);
    }

    [HttpPut]
    public IActionResult ChangeInfo(string storekeeperId, [FromBody] MedalBindingModel model)
    {
        return _adapter.ChangeMedalInfo(storekeeperId, model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string storekeeperId, string id)
    {
        return _adapter.RemoveMedal(storekeeperId, id).GetResponse(Request, Response);
    }
}
