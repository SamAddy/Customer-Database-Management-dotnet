using Component.CustomersDatabase;

namespace Component.CustomersDatabase;
class CustomerDatabase
{
    private List<Customer> _customers = new ();
    private HashSet<string> _emailAddresses;
    private FileHelper? _fileHelper;

    public CustomerDatabase(string filePath)
    {
        _fileHelper = new FileHelper(filePath);
        string[] customerData = _fileHelper.GetAll() ?? Array.Empty<string>();
        _customers = customerData.Length > 0 ? ConvertToCustomers(customerData) : new List<Customer>();
        _emailAddresses = ExtractEmailAddresses(_customers);
    }
    public void AddCustomer(Customer customer)
    {
        if (_emailAddresses.Contains(customer.Email.ToLower()))
        {
            throw new ArgumentException("A customer with the same email address exist already.");
        }

        _customers.Add(customer);
        _emailAddresses.Add(customer.Email.ToLower());
        string newCustomer = customer.ToString() + Environment.NewLine;
        _fileHelper?.WriteToFile(newCustomer);
        Console.WriteLine($"New customer has been added.");
    }

    public void UpdateCustomer(Customer updatedCustomer)
    {
        Console.WriteLine("updates " + updatedCustomer);   
        Customer customer = _customers.First((c) => c.Id.Equals(updatedCustomer.Id));
        Console.WriteLine("This is the customer to be updated: " + customer);
        Console.WriteLine("List of customers" + _customers);
        if (customer != null)
        {
            if (!customer.Email.Equals(updatedCustomer.Email, StringComparison.OrdinalIgnoreCase) && _emailAddresses.Contains(updatedCustomer.Email.ToLower()))
            {
                throw new ArgumentException("A customer with the same email address already exist.");
            }
            customer.FirstName = updatedCustomer.FirstName;
            customer.LastName = updatedCustomer.LastName;
            customer.Email = updatedCustomer.Email;
            customer.Address = updatedCustomer.Address;

            Console.WriteLine(customer);
            UpdateCustomerInFile(customer);
        }
    }
    public Customer? GetCustomerById(string id)
    {
        return _customers.FirstOrDefault();
    }
    private List<Customer> ConvertToCustomers(string[] customerData)
    {
        List<Customer> convertedCustomers = new();
        
        foreach (string data in customerData)
        {
            string[] customerInfo = data.Split(",");

            if (customerInfo.Length == 4)
            {
                string firstName = customerInfo[0];
                string lastName = customerInfo[1];
                string email = customerInfo[2];
                string address = customerInfo[3];

                Customer customer = new Customer(firstName, lastName, email, address);
                convertedCustomers.Add(customer);
            }
        }
        Console.WriteLine("We are here: Converto customers");
        return convertedCustomers;
    }

    private HashSet<string> ExtractEmailAddresses(List<Customer> customers)
    {
        HashSet<string> emailAddresses = new (StringComparer.OrdinalIgnoreCase);

        foreach (Customer customer in _customers)
        {
            emailAddresses.Add(customer.Email.ToLower());
        }
        return emailAddresses;
    }

    private void UpdateCustomerInFile(Customer customer)
    {
        string[]? lines = _fileHelper?.GetAll();
        for (int i = 0; i < lines?.Length; i++)
        {
            if (lines[i].StartsWith(customer.Id))
            {
                lines[i] = customer.ToString();
            }
        }

    }

}