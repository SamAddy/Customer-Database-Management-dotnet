# Customer Database Management

This is a customer database application that can store and retrieve customer information. The application allows users to add, update, delete, and search for customers. In addition, the application is designed to handle large amounts of data efficiently, by utilizing the appropriate data structures and algorithms.

## Features

* <b>Add Customer</b> : Allows users to add a new customer to the database by providing their first name, last name, email address, and address.
* <b>Update Customer</b> : Enables users to update an existing customer's information, including their first name, last name, email address, and address.
* <b>Get Customer By Id</b> : Provides the ability to retrieve customer information by their unique identifier.
* <b>Delete Customer</b> : Allows users to remove a customer from the database using their unique identifier.
* <b>Undo and Redo</b> : Supports undo and redo functionality, allowing users to revert changes or redo previously undone actions.
* <b>File Persistence</b> : Customer data is stored in a CSV file, ensuring that the data remains persistent across application runs.
* <b>Error Handling</b> : Includes error handling and validation to handle cases such as duplicate email addresses and invalid input.

## Prerrequisites

* .NET Core 3.1 or higher

## Getting Started 

1. Clone the repository or download the source code.
2. Open the solution in your preferred development environment.
3. Build the solution to restore dependencies.

## Data Storage
Customer data is stored in a CSV file located at `src/customers.csv`. The file format is as follows:

```
Customer-1, John Doe, john@example.com, 123 Main St
Customer-2, Jane Smith, jane@example.com, 456 Elm St
```
