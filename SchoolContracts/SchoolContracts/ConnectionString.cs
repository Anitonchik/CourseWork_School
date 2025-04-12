namespace SchoolContracts;

public class ConnectionString : IConnectionString
{
    string IConnectionString.ConnectionString => "Server=localhost, 5432;Database=school_rpp;Uid=postgres;Pwd=postgres;";
}
