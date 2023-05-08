using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Collections.Specialized;

namespace CodingTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.AppSettings.Get("connString");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS coding
                    (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    DayOfWeek TEXT,
                    StartTime TEXT,
                    EndTime TEXT,
                    Duration TEXT
                    )";

                tableCmd.ExecuteNonQuery();//Executes the command without returning a value. Only telling it to create a table.

                connection.Close();
            }
            MainMenu.GetUserInput();
        }
    }
}