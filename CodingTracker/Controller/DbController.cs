using Microsoft.Data.Sqlite;
using System.Configuration;
using CodingTracker.View;
using CodingTracker.Models;
using ConsoleTableExt;
using System.Globalization;
using CodingTracker.Helpers;
namespace CodingTracker.Controller;

internal class DbController
{
    private static readonly string connectionString = ConfigurationManager.AppSettings.Get("connString");

    internal static void CreateDb()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var tableCmd = connection.CreateCommand())
            {

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
        }
    }

    internal static string GetCommandText(string viewType)
    {
        string commandText = "";
        switch (viewType.ToLower())
        {
            case "0":
                Console.Clear();
                MainMenu.GetUserInput();
                break;

            case "all":
                Console.Clear();

                commandText = $"SELECT * FROM coding";
                break;

            case "day":
                Console.WriteLine("Enter a day of the week: ");
                var dayOfWeek = Console.ReadLine();

                Console.Clear();
                Console.WriteLine($"Records for every {dayOfWeek}:");
                commandText = $"SELECT * FROM coding WHERE DayOfWeek LIKE '{dayOfWeek}'";
                break;

            case "month":
                Console.WriteLine("Enter the month in number format (eg. 05)");
                var monthOfRecords = Console.ReadLine();

                Console.Clear();
                Console.WriteLine($"Records for the month of {monthOfRecords}:");
                commandText = $"SELECT * FROM coding WHERE SUBSTR(Date, 1, 2) = '{monthOfRecords}'";
                break;

            case "year":
                Console.WriteLine("\n\nEnter the year of the records you want to view: (eg. 2023)");
                var yearOfRecords = Console.ReadLine();

                Console.Clear();
                Console.WriteLine($"Records for the year of {yearOfRecords}:");
                commandText = $"SELECT * FROM coding WHERE SUBSTR(Date, 7, 4) = '{yearOfRecords}'";
                break;

            default:
                Console.WriteLine("Invalid command. Review the options and try again.");
                break;
        }
        return commandText;
    }

    internal static void ManualInsert()
    {
        string date = Helper.GetDateInput("Enter the date for the record you are recording: (eg. mm-dd-yyyy)");

        string dayOfWeek = Helper.GetDayInput("Enter the day of the week for the record: (eg. Monday, Tuesday)\n" +
                                "or type 0 to return to the main menu.");

        string startTime = Helper.GetHourTime("Enter the Start time for the session: (eg. 01:00 AM)" +
                                                "or type 0 to return to the Main Menu.");// pass date as an argument for retreiving the start date/time

        string endTime = Helper.GetHourTime("Enter the End time for the session: (eg. 01:00 AM)" +
                                             "or type 0 to return to the Main Menu.");

        string duration = Helper.CalculateDuration(startTime, endTime);

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var tableCmd = connection.CreateCommand())
            {
                tableCmd.CommandText =
                $"INSERT INTO coding(Date, DayOfWeek, StartTime, EndTime, Duration) VALUES ('{date}', '{dayOfWeek}', '{startTime}', '{endTime}', '{duration}')";

                tableCmd.ExecuteNonQuery();
            }
        }
    }

    internal static void UpdateRecord()
    {
        Console.Clear();
        ViewRecords();

        var recordId = Helper.GetNumberInput("\n\nPlease type the Id of the record you'd like to update " +
                                                    "or 0 to return to the Main Menu");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (var checkCmd = connection.CreateCommand())
            {
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding WHERE Id = {recordId})";
                var checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());//returns 0 for false 1 for true

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nThe following record Id doesnt exist: {recordId}\n\n");
                    connection.Close();
                    UpdateRecord();
                }
            }

            string date = Helper.GetDateInput("Enter the new date for the record you are updating: (eg. mm-dd-yyyy)");

            var dayOfTheWeek = Helper.GetDayInput("Enter the day of the week for the record: (eg. Monday, Tuesday)\n" +
                                "or type 0 to return to the main menu.");

            string startTime = Helper.GetHourTime("Enter the Start time for the session: (eg. 01:00 AM)" +
                                    "or type 0 to return to the Main Menu.");// pass date as an argument for retreiving the start date/time

            string endTime = Helper.GetHourTime("Enter the End time for the session: (eg. 01:00 AM)" +
                                                 "or type 0 to return to the Main Menu.");

            string duration = Helper.CalculateDuration(startTime, endTime);

            using (var tableCmd = connection.CreateCommand())
            {
                tableCmd.CommandText = $"UPDATE coding SET date = '{date}', DayOfWeek = '{dayOfTheWeek}', StartTime = '{startTime}', EndTime = '{endTime}', Duration = '{duration}'" +
                                   $" WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();
            }
        }
    }

    internal static void DeleteRecord()
    {
        Console.Clear();
        ViewRecords();

        var recordId = Helper.GetNumberInput("\n\nPlease type the Id of the record you'd like to delete " +
                                                    "or 0 to return to the Main Menu");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var tableCmd = connection.CreateCommand())
            {
                tableCmd.CommandText = $"DELETE from coding WHERE Id = '{recordId}'";

                var rowCount = tableCmd.ExecuteNonQuery();//returns the amount of rows affected by command

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nThe record you entered does not exist: {recordId}. Press Enter to continue.\n\n");
                    Console.ReadLine();

                    DeleteRecord();
                }
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
        Console.ReadLine();

        var startTime = TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt");
        Console.WriteLine(startTime);

        Console.WriteLine("Press enter to end tracking a session.");
        Console.ReadLine();

        var endTime = TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt");

        Console.WriteLine(endTime);
        var date = DateOnly.FromDateTime(DateTime.Now).ToString("MM-dd-yyyy");

        var dayOfWeek = DateTime.Now.DayOfWeek.ToString();

        var duration = Helper.CalculateDuration(startTime, endTime);

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var tableCmd = connection.CreateCommand())
            {
                tableCmd.CommandText =
                $"INSERT INTO coding(Date, DayOfWeek, StartTime, EndTime, Duration) VALUES ('{date}', '{dayOfWeek}', '{startTime}', '{endTime}', '{duration}')";

                tableCmd.ExecuteNonQuery();
            }
        }
    }

    public static void ViewRecords()
    {
        var summedDuration = TimeSpan.Zero;
        var averageDuration = TimeSpan.Zero;
        int counter = 0;
        Console.Clear();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var tableCmd = connection.CreateCommand())
            {
                Console.WriteLine("\n\nEnter 'all' to view all records, 'day' to view by day\n" +
              "'month' to view by month, or 'year' to view by year. Type 0 to \n" +
              "return to the Main Menu.");
                string viewType = Console.ReadLine();

                tableCmd.CommandText = DbController.GetCommandText(viewType);

                var tableData = new List<CodingSession>();
                var durationData = new List<TimeSpan>();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new CodingSession
                            {
                                Id = reader.GetInt32(0),
                                Date = DateOnly.ParseExact(reader.GetString(1), "MM-dd-yyyy", CultureInfo.InvariantCulture),
                                DayOfWeek = reader.GetString(2),
                                StartTime = TimeOnly.ParseExact(reader.GetString(3), "hh:mm tt", CultureInfo.InvariantCulture),
                                EndTime = TimeOnly.ParseExact(reader.GetString(4), "hh:mm tt", CultureInfo.InvariantCulture),
                                Duration = TimeSpan.Parse(reader.GetString(5))
                            });
                        durationData.Add(TimeSpan.Parse(reader.GetString(5)));
                        counter++;
                    }
                    foreach (TimeSpan time in durationData)
                    {
                        summedDuration += time;
                    }
                    averageDuration = summedDuration / durationData.Count;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\n\nNo records found.");
                }

                ConsoleTableBuilder
                    .From(tableData)
                    .ExportAndWriteLine();
            }

            Console.WriteLine($"Total duration spent coding: {summedDuration}");
            Console.WriteLine($"Average time spent per session: {averageDuration}");
        }
        Console.WriteLine($"Total Entries: {counter}");
        Console.WriteLine("=======================================");
        Console.WriteLine("Press Enter to Continue");
        Console.ReadLine();
    }
}