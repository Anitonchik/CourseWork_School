using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolContracts.AdapterContracts;
using SchoolContracts.BindingModels;
using SchoolWebApi.Adapters;

namespace SchoolWebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class WorkerController : ControllerBase
{
    IWorkerAdapter _adapter;

    public WorkerController(IWorkerAdapter adapter) => _adapter = adapter;

    [HttpGet]
    public IActionResult GetUserByLogin(string login)
    {

        return _adapter.GetUserByLogin(login).GetResponse(Request, Response);
    }

    /*[HttpGet("{id}")]
     public IActionResult GetRecordBuId(string workerId, string data)
     {
         return _adapter.GetElement(workerId, data).GetResponse(Request, Response);
     }*/

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
