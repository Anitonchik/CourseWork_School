using SchoolContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.BusinessLogicsContracts;

public interface IWorkerBuisnessLogicContract
{
    WorkerDataModel GetWorkerByLogin(string login);
    WorkerDataModel GetWorkerByMail(string mail);
    void InsertWorker(WorkerDataModel workerDataModel);
    void UpdateWorker(WorkerDataModel workerDataModel);
    void DeleteWorker(string id);
}
