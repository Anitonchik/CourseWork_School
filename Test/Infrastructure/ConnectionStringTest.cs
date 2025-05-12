using SchoolContracts.Infrastructure;

namespace SchoolTests.Infrastructure;

internal class ConnectionStringTest : IConnectionString
{
    string IConnectionString.ConnectionString => "Server=localhost, 5432;Database=school_rpp_2;Uid=postgres;Pwd=postgres;";
    //string IConnectionString.ConnectionString => "Server=localhost, 5432;Database=school;Uid=postgres;Pwd=12345678;";
}
