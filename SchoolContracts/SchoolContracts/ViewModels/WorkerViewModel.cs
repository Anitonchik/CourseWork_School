using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.ViewModels;

public class WorkerViewModel
{
    public required string Id { get; set; }
    public required string FIO { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
    public required string Mail { get; set; }
}
