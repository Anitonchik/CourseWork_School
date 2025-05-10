using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.AdapterContracts;

public interface IWorkerAdapter
{
    WorkerOperationResponse RegisterWorker(string workerId, WorkerBindingModel workerModel);

    WorkerOperationResponse ChangeWorkerInfo(string workerId, WorkerBindingModel workerModel);

    WorkerOperationResponse RemoveWorker(string workerId, string id);
}
