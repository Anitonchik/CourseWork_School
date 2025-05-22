using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolContracts.BindingModels;
using SchoolWebApi.Adapters;

namespace SchoolWebApi.Controllers.Users;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class WorkerController : ControllerBase
{
    UserWorkerAdapter _adapter;

    public WorkerController(UserWorkerAdapter adapter) => _adapter = adapter;

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

    [HttpGet]
    public IActionResult GetUserByLogin(string login)
    {

        return _adapter.GetUserByLogin(login).GetResponse(Request, Response);
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
