﻿using SchoolContracts.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.DataModels;

public class InterestDataModel(string id,string workerId, string interestName, string description, List<InterestMaterialDataModel> InterestMaterials)
{
    public string Id { get; private set; } = id;
    public string WorkerId { get; private set; } = workerId;
    public string InterestName { get; private set; } = interestName;
    public string Description { get; private set; } = description;
    public List<InterestMaterialDataModel> Materials { get; private set; } = InterestMaterials;
    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");

        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");

        if (WorkerId.IsEmpty())
            throw new ValidationException("Field WorkerId is empty");

        if (!WorkerId.IsGuid())
            throw new ValidationException("The value in the field WorkerId is not a unique identifier");

        if (InterestName.IsEmpty())
            throw new ValidationException("Field InterestName is empty");
    }
}

