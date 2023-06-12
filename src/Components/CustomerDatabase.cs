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
        _fileHelper?.WriteToFile(newCustomer.ToString());
        Console.WriteLine($"New customer has been added.");
    }

    public void UpdateCustomer(Customer updatedCustomer)
    {
        Customer customer = _customers.FirstOrDefault((c) => c.Id.Equals(updatedCustomer.Id));
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
            UpdateCustomerInFile(customer);
            Console.WriteLine("Customer info updated in file successfully.");
        }
    }

    public void DeleteCustomer(string id) 
    {
        Customer customer = _customers.FirstOrDefault((c) => c.Id == id);
        if (customer != null)
        {
            _customers.Remove(customer);
            _emailAddresses.Remove(customer.Email.ToLower());
        }
    }
    public Customer? GetCustomerById(string id)
    {
        return _customers.FirstOrDefault((customer) => customer.Id == id);
    }
    private List<Customer> ConvertToCustomers(string[] customerData)
    {
        List<Customer> convertedCustomers = new();
        
        foreach (string data in customerData)
        {
            string[] customerInfo = data.Split(",");

            if (customerInfo.Length == 5)
            {
                string id = customerInfo[0];
                string firstName = customerInfo[1];
                string lastName = customerInfo[2];
                string email = customerInfo[3];
                string address = customerInfo[4];

                Customer customer = new Customer(id, firstName, lastName, email, address);
                convertedCustomers.Add(customer);
            }
        }
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
                Console.WriteLine("customer to be updated found: " + lines[i]);
                lines[i] = customer.ToString();
                break;
            }
        }
        lines = lines?.Where(line => !line.StartsWith(customer.Id)).ToArray();

        _fileHelper?.WriteToFile(string.Join(Environment.NewLine, lines));
    }
}