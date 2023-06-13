using Component.CustomersDatabase;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Customer Database Program");
        CustomerDatabase customerDatabase = new CustomerDatabase("src/customers.csv");

        Customer customer1 = new Customer("Customer-1", "John", "Doe", "john@example.com", "123 Main St");
        Customer customer2 = new Customer("Customer-2", "Jane", "Smith", "jane@example.com", "456 Elm St");
        Customer customer3 = new Customer("Customer-3", "Jelly", "Smith", "jelly@example.com", "321 Wall St");
        Customer updated1 = new Customer("Customer-2", "Ellen", "Doe", "newemail@example.com", "789 Oak St");
        string customerId = "Customer-1";

        customerDatabase.AddCustomer(customer1);
        customerDatabase.AddCustomer(customer2);
        // customerDatabase.UpdateCustomer(updated1);
        customerDatabase.GetCustomerById(customerId);
        customerDatabase.DeleteCustomer(customerId);
        // customerDatabase.AddCustomer(customer3);
    }
}