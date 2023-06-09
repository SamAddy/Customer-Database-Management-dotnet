namespace Component.CustomersDatabase;
class Customer 
{
    private static int idCounter = 1;
    private string _id;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }

    public Customer(string firstName, string lastName, string email, string address)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Address = address;
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
        return $"{_id} \n Name: {FirstName}{LastName}\n Email: {Email}\n Address: {Address}\n";
    }
}
