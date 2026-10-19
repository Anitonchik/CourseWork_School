using SchoolContracts.Infrastructure;

namespace SchoolContracts;

public class ConnectionString : IConnectionString
{
    string IConnectionString.ConnectionString => "Server=localhost, 5432;Database=school_rpp_2;Uid=postgres;Pwd=postgres;";
    //string IConnectionString.ConnectionString => "Server=localhost, 5432;Database=school_rpp;Uid=postgres;Pwd=12345678;";
}
