using SchoolContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.BusinessLogicsContracts;

public interface IWorkerBuisnessLogicContract
{
    List<WorkerDataModel> GetAllWorkers();
    WorkerDataModel GetWorkerByData(string data);
    void InsertWorker(WorkerDataModel workerDataModel);
    void UpdateWorker(WorkerDataModel workerDataModel);
    void DeleteWorker(string id);
}
