using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolContracts.BindingModels;
using SchoolWebApi.Adapters;

namespace SchoolWebApi.Controllers.Users;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class StorekeeperAccauntController : ControllerBase
{
    UserStorekeeperAdapter _adapter;

    public StorekeeperAccauntController(UserStorekeeperAdapter adapter) => _adapter = adapter;

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

    [HttpPost]
    public IActionResult Register([FromBody] StorekeeperBindingModel model)
    {

        return _adapter.RegisterStorekeeper(model).GetResponse(Request, Response);
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
