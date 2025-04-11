using SchoolContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.StoragesContracts;

public interface IWorkerStorageContract
{
    List<WorkerDataModel> GetList();
    WorkerDataModel? GetElementById(string id);
    WorkerDataModel? GetElementByFIO(string fio);
    WorkerDataModel? GetElementByLogin(string phoneLogin);
    WorkerDataModel? GetElementByMail(string phoneMail);
    void AddElement(WorkerDataModel workerDataModel);
    void UpdElement(WorkerDataModel workerDataModel);
    void DelElement(string id);
}
