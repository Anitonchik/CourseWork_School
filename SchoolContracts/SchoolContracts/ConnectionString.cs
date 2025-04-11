namespace SchoolContracts;

internal class ConnectionString : IConnectionString
{
    string IConnectionString.ConnectionString => "Server=localhost, 5432;Database=schoolrpp;Uid=postgres;Pwd=postgres;";
}
