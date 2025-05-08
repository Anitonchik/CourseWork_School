namespace SchoolContracts.Exceptions;

public class UnauthorizedAccessException : Exception
{
    public UnauthorizedAccessException(string id) : base($"Entry does not belong to user with id = {id}") { }
}
