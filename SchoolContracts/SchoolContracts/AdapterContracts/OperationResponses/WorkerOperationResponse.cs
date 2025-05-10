using SchoolContracts.Infrastructure;
using SchoolContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.AdapterContracts.OperationResponses;

public class WorkerOperationResponse : OperationResponse
{
    public static WorkerOperationResponse OK(WorkerViewModel data) => OK<WorkerOperationResponse, WorkerViewModel>(data);

    public static WorkerOperationResponse NoContent() => NoContent<WorkerOperationResponse>();

    public static WorkerOperationResponse BadRequest(string message) => BadRequest<WorkerOperationResponse>(message);

    public static WorkerOperationResponse NotFound(string message) => NotFound<WorkerOperationResponse>(message);

    public static WorkerOperationResponse InternalServerError(string message) => InternalServerError<WorkerOperationResponse>(message);
}
