using Microsoft.Data.Sqlite;

namespace CodingTracker
{
    internal class Program
    {
        public static string ConnectionString { get; set; } = @"Data Source=coding-Tracker.db";
        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(ConnectionString))
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