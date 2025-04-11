using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.DataModels;

public class InterestDataModel(string id,string workerId, string interestName, string description)
{
    public string Id { get; private set; } = id;
    public string WorkerId { get; private set; } = workerId;
    public string InterestName { get; private set; } = interestName;
    public string Description { get; private set; } = description;
}

