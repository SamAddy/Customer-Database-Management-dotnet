namespace Component.CustomersDatabase;
class Customer 
{
    private string _id;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Id 
    { 
        get { return _id; } 
        private set { _id = value; } 
    }

    public Customer(string id, string firstName, string lastName, string email, string address)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Address = address;
    }

    public override string ToString()
    {
        return $"{Id}, {FirstName} {LastName}, {Email}, {Address}";
    }
}
