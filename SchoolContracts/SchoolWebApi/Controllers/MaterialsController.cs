using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolContracts.AdapterContracts;
using SchoolContracts.BindingModels;

namespace SchoolWebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class MaterialsController(IMaterialAdapter adapter) : ControllerBase
{
    private readonly IMaterialAdapter _adapter = adapter;

    [HttpGet]
    public IActionResult GetAllRecords(string storekeeperId)
    {
        return _adapter.GetList(storekeeperId).GetResponse(Request, Response);
    }

    [HttpGet("{data}")]
    public IActionResult GetRecord(string storekeeperId, string data)
    {
        return _adapter.GetElement(storekeeperId, data).GetResponse(Request, Response);
    }

    [HttpPost]
    public IActionResult Register(string storekeeperId, [FromBody] MaterialBindingModel model)
    {
        return _adapter.RegisterMaterial(storekeeperId, model).GetResponse(Request, Response);
    }

    [HttpPut]
    public IActionResult ChangeInfo(string storekeeperId, [FromBody] MaterialBindingModel model)
    {
        return _adapter.ChangeMaterialInfo(storekeeperId, model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string storekeeperId, string id)
    {
        return _adapter.RemoveMaterial(storekeeperId, id).GetResponse(Request, Response);
    }
}
