using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.BindingModels;

public class WorkerBindingModel
{
    public string? Id { get; set; }
    public string? FIO { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Mail { get; set; }
}
