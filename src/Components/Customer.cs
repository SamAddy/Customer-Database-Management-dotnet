class Customer 
{
    private static int idCounter = 1;
    private string _id;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public Customer(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        _id = GenerateId();
    }
    private string GenerateId()
    {
        string id = $"Customer-{idCounter}";
        idCounter++;
        return id;
    }

    public override string ToString()
    {
        return $"{_id} First Name: {FirstName} Last Name: {LastName} Email: {Email}";
    }
}
