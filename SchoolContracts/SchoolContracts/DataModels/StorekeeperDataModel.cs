namespace SchoolContracts.DataModels;

public class StorekeeperDataModel (string id, string fio, string login, string password, string mail)
{
    public string Id { get; private set; } = id;
    public string FIO { get; private set; } = fio;
    public string Login { get; private set; } = login;
    public string Password { get; private set; } = password;
    public string Mail { get; private set; } = mail;

}
