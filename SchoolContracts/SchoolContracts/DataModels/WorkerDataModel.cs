﻿using SchoolContracts.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.DataModels;

public class WorkerDataModel(string id, string fio, string login, string password, string mail)
{
    public string Id { get; private set; } = id;
    public string FIO { get; private set; } = fio;
    public string Login { get; private set; } = login;
    public string Password { get; private set; } = password;
    public string Mail { get; private set; } = mail;

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");

        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");

        if (FIO.IsEmpty())
            throw new ValidationException("Field FIO is empty");

        if (Login.IsEmpty())
            throw new ValidationException("Field Login is empty");

        if (Login.Length >= 50)
            throw new ValidationException("Field Login is greater than or equal to 50");

        if (Password.IsEmpty())
            throw new ValidationException("Field Password is empty");

        if (Mail.IsEmpty())
            throw new ValidationException("Field Mail is empty");
    }
}
