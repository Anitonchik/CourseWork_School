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
    WorkerOperationResponse GetUserByLogin(string login);
    WorkerOperationResponse RegisterWorker( WorkerBindingModel workerModel);

    WorkerOperationResponse ChangeWorkerInfo( WorkerBindingModel workerModel);

    WorkerOperationResponse RemoveWorker(string id);
}
