using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker
{
    internal class DbManager
    {
        private readonly static string connectionString = ConfigurationManager.AppSettings.Get("connString");
        internal static void ManualInsert()
        {
            string date = Helpers.GetDateInput("Enter the date for the record you are recording: (eg. mm-dd-yyyy)");

            string dayOfWeek = DateTime.Now.DayOfWeek.ToString();

            string startTime = Helpers.GetHourTime("Enter the Start time for the session: (eg. 01:00 AM)" +
                                                    "or type 0 to return to the Main Menu.");// pass date as an argument for retreiving the start date/time

            string endTime = Helpers.GetHourTime("Enter the End time for the session: (eg. 01:00 AM)" +
                                                 "or type 0 to return to the Main Menu.");

            string duration = Helpers.CalculateDuration(startTime, endTime);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"INSERT INTO coding(Date, DayOfWeek, StartTime, EndTime, Duration) VALUES ('{date}', '{dayOfWeek}', '{startTime}', '{endTime}', '{duration}')";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
            //Console.Clear();
        }

        internal static void UpdateRecord()
        {
            Console.Clear();

            View.ViewRecords();

            var recordId = Helpers.GetNumberInput("\n\nPlease type the Id of the record you'd like to update " +
                                                "or 0 to return to the Main Menu");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding WHERE Id = {recordId})";
                var checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());//returns 0 for false 1 for true

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nThe following record Id doesnt exist: {recordId}\n\n");
                    connection.Close();
                    UpdateRecord();
                }

                string date = Helpers.GetDateInput("Enter the new date for the record you are updating: (eg. mm-dd-yyyy)");

                string dayOfWeek = DateTime.Now.DayOfWeek.ToString();

                string startTime = Helpers.GetHourTime("Enter the Start time for the session: (eg. 01:00 AM)" +
                                        "or type 0 to return to the Main Menu.");// pass date as an argument for retreiving the start date/time

                string endTime = Helpers.GetHourTime("Enter the End time for the session: (eg. 01:00 AM)" +
                                                     "or type 0 to return to the Main Menu.");

                string duration = Helpers.CalculateDuration(startTime, endTime);

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE coding SET date = '{date}', DayOfWeek = '{dayOfWeek}' StartTime = '{startTime}', EndTime = '{endTime}', Duration = '{duration}'" +
                                       $" WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal static void DeleteRecord()
        {
            Console.Clear();
            View.ViewRecords();

            var recordId = Helpers.GetNumberInput("\n\nPlease type the Id of the record you'd like to delete " +
                                                        "or 0 to return to the Main Menu");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE from coding WHERE Id = '{recordId}'";

                var rowCount = tableCmd.ExecuteNonQuery();//returns the amount of rows affected by command

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nThe record you entered does not exist: {recordId}. Press Enter to continue.\n\n");
                    Console.ReadLine();

                    DeleteRecord();
                }
            }

            Console.WriteLine($"The following record Id was deleted: {recordId}. Press Enter to return to main menu.\n\n");
            Console.ReadLine();

            MainMenu.GetUserInput();
        }

        internal static void StopwatchInsert()
        {
            Console.Clear();
            Console.WriteLine("\n\nStopwatch session Menu");
            Console.WriteLine("Press enter to start tracking a session");

            Console.ReadLine() ;
            var startTime = TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt");
            Console.WriteLine(startTime);

            Console.WriteLine("Press enter to end tracking a session.");

            Console.ReadLine();
            var endTime = TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt");

            Console.WriteLine(endTime);
            var date = DateOnly.FromDateTime(DateTime.Now).ToString("MM-dd-yyyy");

            string dayOfWeek = DateTime.Now.DayOfWeek.ToString();

            var duration = Helpers.CalculateDuration(startTime, endTime);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"INSERT INTO coding(Date, DayOfWeek, StartTime, EndTime, Duration) VALUES ('{date}', '{dayOfWeek}', '{startTime}', '{endTime}', '{duration}')";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}