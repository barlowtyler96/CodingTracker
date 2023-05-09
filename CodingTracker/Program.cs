using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Collections.Specialized;
using System.Drawing.Text;

namespace CodingTracker
{
    internal class Program
    {
        private static readonly string connectionString = ConfigurationManager.AppSettings.Get("connString");
        static void Main(string[] args)
        {
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
            }
            MainMenu.GetUserInput();
        }
    }
}