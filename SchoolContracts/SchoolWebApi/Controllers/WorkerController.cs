using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolContracts.AdapterContracts;
using SchoolContracts.BindingModels;
using SchoolWebApi.Adapters;

namespace SchoolWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class WorkersController : ControllerBase
{
    IWorkerAdapter _adapter;

    public WorkersController(IWorkerAdapter adapter) => _adapter = adapter;

    /* [HttpGet]
     public IActionResult GetAllRecords(string storekeeperId)
     {
         return _adapter.GetList(storekeeperId).GetResponse(Request, Response);
     }

     [HttpGet("{id}")]
     public IActionResult GetRecordBuId(string storekeeperId, string data)
     {
         return _adapter.GetElement(storekeeperId, data).GetResponse(Request, Response);
     }*/

    [HttpGet("GetUserByLogin")]
    public IActionResult GetUserByLogin(string login)
    {

        return _adapter.GetUserByLogin(login).GetResponse(Request, Response);
    }

    /*
    [HttpPut]
    public IActionResult ChangeInfo(string storekeeperId, [FromBody] CircleBindingModel model)
    {
        return _adapter.ChangeCircleInfo(storekeeperId, model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string storekeeperId, string id)
    {
        return _adapter.RemoveCircle(storekeeperId, id).GetResponse(Request, Response);
    }*/
}
