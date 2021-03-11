using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ConsoleSQLTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestDB1;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    // Открываем подключение
                    connection.Open();
                    Console.WriteLine("Подключение открыто");

                    string sqlExpression = "INSERT INTO Users (Name, Description) VALUES ('Vasya', 'Security guy')";

                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    command.Connection = connection;
                    int number = command.ExecuteNonQuery();
                    Console.WriteLine("Добавлено объектов: {0}", number);


                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    // закрываем подключение
                    connection.Close();
                    Console.WriteLine("Подключение закрыто...");
                }
            }

            Console.Read();
        }
    }
}
