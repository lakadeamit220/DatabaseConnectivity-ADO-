using System;
using System.Data.SqlClient;

namespace DatabaseConnectivity_ADO_
{
    internal class Program
    {
        // Connection string to the SQL Server database
        private static string connectionString = "Server=AMITLAKADE\\SQLEXPRESS;Database=sqldb;Trusted_Connection=True;";

        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                // Display menu options
                Console.WriteLine("Select an operation:");
                Console.WriteLine("1. Insert a record");
                Console.WriteLine("2. Update a record");
                Console.WriteLine("3. Delete a record");
                Console.WriteLine("4. Display all records");
                Console.WriteLine("5. Exit");
                Console.WriteLine("Enter the number of your choice:");

                // Get user choice
                string choice = Console.ReadLine();

                // Perform the selected operation
                switch (choice)
                {
                    case "1":
                        // Insert a record
                        Console.WriteLine("Enter ProductName:");
                        string insertProductName = Console.ReadLine();
                        Console.WriteLine("Enter Category:");
                        string insertCategory = Console.ReadLine();
                        Console.WriteLine("Enter Price:");
                        decimal insertPrice = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("Enter Quantity:");
                        int insertQuantity = Convert.ToInt32(Console.ReadLine());
                        InsertRecord(insertProductName, insertCategory, insertPrice, insertQuantity);
                        break;

                    case "2":
                        // Update a record
                        Console.WriteLine("Enter ProductID of the record to update:");
                        int updateProductId = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter new ProductName:");
                        string updateProductName = Console.ReadLine();
                        Console.WriteLine("Enter new Category:");
                        string updateCategory = Console.ReadLine();
                        Console.WriteLine("Enter new Price:");
                        decimal updatePrice = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("Enter new Quantity:");
                        int updateQuantity = Convert.ToInt32(Console.ReadLine());
                        UpdateRecord(updateProductId, updateProductName, updateCategory, updatePrice, updateQuantity);
                        break;

                    case "3":
                        // Delete a record
                        Console.WriteLine("Enter ProductID of the record to delete:");
                        int deleteProductId = Convert.ToInt32(Console.ReadLine());
                        DeleteRecord(deleteProductId);
                        break;

                    case "4":
                        // Display all records
                        DisplayAllRecords();
                        break;

                    case "5":
                        // Exit the application
                        exit = true;
                        Console.WriteLine("Exiting the application.");
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                // Add a pause before redisplaying the menu
                if (!exit)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        // Method to insert a new record into the Products table
        static void InsertRecord(string productName, string category, decimal price, int quantity)
        {
            string query = "INSERT INTO Products (ProductName, Category, Price, Quantity) VALUES (@ProductName, @Category, @Price, @Quantity)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection established to insert record.");

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", productName);
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Quantity", quantity);

                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Record inserted successfully. Rows affected: {rowsAffected}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while inserting the record: {ex.Message}");
                }
            }
        }

        // Method to update an existing record in the Products table
        static void UpdateRecord(int productId, string productName, string category, decimal price, int quantity)
        {
            string query = "UPDATE Products SET ProductName = @ProductName, Category = @Category, Price = @Price, Quantity = @Quantity WHERE ProductID = @ProductID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection established to update record.");

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", productId);
                        command.Parameters.AddWithValue("@ProductName", productName);
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Quantity", quantity);

                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Record updated successfully. Rows affected: {rowsAffected}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while updating the record: {ex.Message}");
                }
            }
        }

        // Method to delete a record from the Products table
        static void DeleteRecord(int productId)
        {
            string query = "DELETE FROM Products WHERE ProductID = @ProductID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection established to delete record.");

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", productId);

                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Record deleted successfully. Rows affected: {rowsAffected}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while deleting the record: {ex.Message}");
                }
            }
        }

        // Method to display all records from the Products table
        static void DisplayAllRecords()
        {
            string query = "SELECT ProductID, ProductName, Category, Price, Quantity, CreatedAt FROM Products";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection established to retrieve records.");

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                // Display headers with alignment
                                Console.WriteLine($"{"ProductID",-10} | {"ProductName",-20} | {"Category",-15} | {"Price",-10} | {"Quantity",-10} | {"CreatedAt",-20}");
                                Console.WriteLine(new string('-', 80));

                                while (reader.Read())
                                {
                                    int productId = reader.GetInt32(0);
                                    string productName = reader.GetString(1);
                                    string category = reader.IsDBNull(2) ? "N/A" : reader.GetString(2);
                                    decimal price = reader.GetDecimal(3);
                                    int quantity = reader.GetInt32(4);
                                    DateTime createdAt = reader.GetDateTime(5);

                                    // Display records with alignment
                                    Console.WriteLine($"{productId,-10} | {productName,-20} | {category,-15} | {price,10:C} | {quantity,10} | {createdAt,-20}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No records found.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while retrieving records: {ex.Message}");
                }
            }
        }
    }
}
