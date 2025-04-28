namespace SchoolContracts.Exceptions;

public class ConnectElementsNotFoundException : Exception
{
    public string FirstValue { get; private set; }
    public string SecondValue { get; private set; }
    public ConnectElementsNotFoundException(string value1, string value2) : base($"Many to many relationship not found at values {value1} and {value2}")
    {
        FirstValue = value1;
        SecondValue = value2;
    }
}
