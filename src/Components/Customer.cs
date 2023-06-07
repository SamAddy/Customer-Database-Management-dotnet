class Customer 
{
    private static int idCounter = 1;
    private string _id;
    private string _firstName;
    private string _lastName;
    private string _email;

    public string FirstName
    {
        set => _firstName = value;
        get => _firstName;
    }

    public string LastName
    {
        set => _lastName = value;
        get => _lastName;
    }

    public string Email 
    {
        set => _email = value;
        get => _email;
    }

    private string GenerateId()
    {
        string id = $"Customer-${idCounter}";
        idCounter++;
        return id;
    }
}
