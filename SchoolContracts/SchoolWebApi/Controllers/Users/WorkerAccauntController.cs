using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolContracts.BindingModels;
using SchoolWebApi.Adapters;

namespace SchoolWebApi.Controllers.Users;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class WorkerAccauntController : ControllerBase
{
    UserWorkerAdapter _adapter;

    public WorkerAccauntController(UserWorkerAdapter adapter) => _adapter = adapter;

    /* [HttpGet]
     public IActionResult GetAllRecords(string workerId)
     {
         return _adapter.GetList(workerId).GetResponse(Request, Response);
     }

     [HttpGet("{id}")]
     public IActionResult GetRecordBuId(string workerId, string data)
     {
         return _adapter.GetElement(workerId, data).GetResponse(Request, Response);
     }*/

    [HttpPost]
    public IActionResult Register([FromBody] WorkerBindingModel model)
    {

        return _adapter.RegisterWorker(model).GetResponse(Request, Response);
    }

    /*
    [HttpPut]
    public IActionResult ChangeInfo(string workerId, [FromBody] WorkerBindingModel model)
    {
        return _adapter.ChangeWorkerInfo(workerId, model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string workerId, string id)
    {
        return _adapter.RemoveWorker(workerId, id).GetResponse(Request, Response);
    }*/
}
