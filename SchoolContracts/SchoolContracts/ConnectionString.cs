﻿namespace SchoolContracts;

public class ConnectionString : IConnectionString
{
    string IConnectionString.ConnectionString => "Server=localhost, 5432;Database=school_rpp_3;Uid=postgres;Pwd=postgres;";
    //string IConnectionString.ConnectionString => "Server=localhost, 5432;Database=school;Uid=postgres;Pwd=12345678;";
}
