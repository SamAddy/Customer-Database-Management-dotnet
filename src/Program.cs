using Component.CustomersDatabase;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Customer Database Program");
        
        Customer customer1 = new Customer("Bismark", "Opoku", "opoku.bismark@mail.me", "Bermuda 4C, 79801");
        // Customer customer2 = new Customer("Sherif", "Suleman", "opoku.bismark@mail.me", "Bermuda 4C, 79801");
        Customer update1 = new Customer("Obisi", "Opoku", "opoku.bismark@mail.me", "Bermuda 4C, 79801");

        CustomerDatabase customerDatabase = new CustomerDatabase("src/customers.csv");
        Console.WriteLine(customer1);
        customerDatabase.AddCustomer(customer1);
        customerDatabase.UpdateCustomer(update1);
        // customerDatabase.AddCustomer(customer2);
        // // Console.WriteLine(customerDatabase);
    }
}