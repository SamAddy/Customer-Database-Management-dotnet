using Component.CustomersDatabase;

namespace Component.CustomersDatabase;
class CustomerDatabase
{
    private List<Customer> _customers = new ();
    private HashSet<string> _emailAddresses;
    private FileHelper? _fileHelper;
    private Stack<Action> _undoStack;
    private Stack<Action> _redoStack;

    public CustomerDatabase(string filePath)
    {
        _fileHelper = new FileHelper(filePath);
        string[] customerData = _fileHelper.GetAll() ?? Array.Empty<string>();
        _customers = customerData.Length > 0 ? ConvertToCustomers(customerData) : new List<Customer>();
        _emailAddresses = ExtractEmailAddresses(_customers);
        _undoStack = new Stack<Action>();
        _redoStack = new Stack<Action>();
    }
    
    public void AddCustomer(Customer customer)
    {
        if (_emailAddresses.Contains(customer.Email.ToLower()))
        {
            throw new ArgumentException("A customer with the same email address exist already.");
        }
        _customers.Add(customer);
        _emailAddresses.Add(customer.Email.ToLower());
        Action addAction = new Action(ActionType.Add, customer);
        _undoStack.Push(addAction);
        _fileHelper?.WriteToFile(customer.ToString());
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
            Customer prevState = new (customer.Id, customer.FirstName, customer.LastName, customer.Email, customer.Address);
            Action updateAction = new Action(ActionType.Update, prevState);
            _undoStack.Push(updateAction);
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
            Action deleteAction = new Action(ActionType.Delete, customer);
            _undoStack.Push(deleteAction);
            _customers.Remove(customer);
            _emailAddresses.Remove(customer.Email.ToLower());
        }
        UpdateFile(customer);
    }

    public Customer? GetCustomerById(string id)
    {
        return _customers.FirstOrDefault((customer) => customer.Id == id);
    }

    public void Undo()
    {
        if (_undoStack.Count > 0)
        {
            Action action = _undoStack.Pop();
            _redoStack.Push(action);

            switch (action.Type)
            {
                case ActionType.Add:
                    Customer addedCustomer = action.Customer;
                    _customers.Remove(addedCustomer);
                    _emailAddresses.Remove(addedCustomer.Email.ToLower());
                    break;
                case ActionType.Update:
                    Customer prevState = action.Customer;
                    Customer updatedCustomer = _customers.FirstOrDefault((c) => c.Id == prevState.Id);
                    if (updatedCustomer != null)
                    {
                        updatedCustomer.FirstName = prevState.FirstName;
                        updatedCustomer.LastName = prevState.LastName;
                        updatedCustomer.Email = prevState.Email;
                        updatedCustomer.Address = prevState.Address;
                    }
                    break;
                case ActionType.Delete:
                    Customer deletedCustomer = action.Customer;
                    _customers.Add(deletedCustomer);
                    _emailAddresses.Add(deletedCustomer.Email.ToLower());
                    break;
            }
        } 
    }

    public void Redo()
    {
        if (_redoStack.Count > 0)
        {
            Action action = _redoStack.Pop();
            _undoStack.Push(action);

            switch (action.Type)
            {
                case ActionType.Add:
                    Customer addedCustomer = action.Customer;
                    _customers.Add(addedCustomer);
                    _emailAddresses.Add(addedCustomer.Email.ToLower());
                    break;
                case ActionType.Update:
                    Customer updatedCustomer = action.Customer;
                    Customer customer = _customers.FirstOrDefault((c) => c.Id == updatedCustomer.Id);
                    if (customer != null)
                    {
                        customer.FirstName = updatedCustomer.FirstName;
                        customer.LastName = updatedCustomer.LastName;
                        customer.Email = updatedCustomer.Email;
                        customer.Address = updatedCustomer.Address;
                    }
                    break;
                case ActionType.Delete:
                    Customer deletedCustomer = action.Customer;
                    _customers.Remove(deletedCustomer);
                    _emailAddresses.Remove(deletedCustomer.Email.ToLower());
                    break;
            }
        }
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

    private void UpdateFile(Customer customer)
    {
        string[] lines = _fileHelper.GetAll();
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith(customer.Id))
            {
                Console.WriteLine("Customer to be deleted found: " + lines[i]);
                lines[i] = string.Empty;
                break;
            }
        }
        string newContent = string.Join(Environment.NewLine, lines);
        _fileHelper.ReplaceFileContent(newContent);
        Console.WriteLine("New content added");
    }

    public enum ActionType
    {
        Add, 
        Update,
        Delete
    }

    public class Action
    {
        public ActionType Type { get; }
        public Customer Customer { get; }

        public Action(ActionType type, Customer customer)
        {
            Type = type;
            Customer = customer;
        }
    }
}