using ConsoleTableExt;
using Microsoft.Data.Sqlite;
using System.Globalization;


namespace CodingTracker
{
    internal class View
    {
        // create a list of Coding Sessions and display using ConsoleTableExt

        public static void ViewRecords()
        {

            int counter = 0;
            Console.Clear();
            using (var connection = new SqliteConnection(Program.ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();


                string viewType = Helpers.GetViewType();

                switch (viewType.ToLower())
                {
                    case "0":
                        Console.Clear();
                        return;

                    case "all":
                        tableCmd.CommandText = $"SELECT * FROM coding";
                        break;

                    case "day":
                        var dayOfRecords = Console.ReadLine();
                        tableCmd.CommandText = $"SELECT * FROM coding WHERE Date LIKE  '%{dayOfRecords}%'"; // make this display day of week not number of month
                        break;

                    case "week":
                        var weekOfRecords = Console.ReadLine();
                        tableCmd.CommandText = $"SELECT * FROM coding WHERE Date LIKE  '%{weekOfRecords}%'";
                        break;

                    case "year":
                        Console.WriteLine("\n\nEnter the year of the records you want to view: ");
                        var yearOfRecords = Console.ReadLine();
                        tableCmd.CommandText = $"SELECT * FROM coding WHERE Date LIKE '%{yearOfRecords}%'";
                        break;

                    default:
                        Console.WriteLine("Invalid command. Review the options and try again.");
                        Helpers.GetViewType();
                        break;

                }

                var tableData = new List<CodingSession>();
                var durationData = new List<TimeSpan>();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                //Add specified records to list of Sessions
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new CodingSession
                            {
                                Id = reader.GetInt32(0), //returns values of column(i) specified
                                Date = DateOnly.ParseExact(reader.GetString(1), "MM-dd-yyyy", CultureInfo.InvariantCulture),
                                StartTime = TimeOnly.ParseExact(reader.GetString(2), "hh:mm tt", CultureInfo.InvariantCulture), // TODO change cultureinfo?
                                EndTime = TimeOnly.ParseExact(reader.GetString(3), "hh:mm tt", CultureInfo.InvariantCulture),
                                Duration = TimeSpan.Parse(reader.GetString(4))
                            });
                        durationData.Add(TimeSpan.Parse(reader.GetString(4))); // TODO parse this somehow
                        counter++;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\n\nNo records found.");
                }

                connection.Close();

                ConsoleTableBuilder
                    .From(tableData)
                    .ExportAndWriteLine();

                // Add/Average elements in TimeSpan list
                var summedDuration = TimeSpan.Zero;

                foreach (TimeSpan time in durationData)
                {
                    summedDuration += time;
                }

                var averageDuration = summedDuration / durationData.Count;

                Console.WriteLine($"Total duration spent coding: {summedDuration}");
                Console.WriteLine($"Average time spent per session: {averageDuration}");
            }
            Console.WriteLine($"Total Entries: {counter}");
            Console.WriteLine("=======================================");
            Console.WriteLine("Press Enter to Return to menu");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
