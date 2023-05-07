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
            var summedDuration = TimeSpan.Zero;
            var averageDuration = TimeSpan.Zero;
            int counter = 0;
            Console.Clear();

            using (var connection = new SqliteConnection(Program.ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                Console.WriteLine("Enter 'all' to view all records, 'day' to view by day" +
                  "'month' to view by month, or 'year' to view by year. Type 0 to " +
                  "return to the Main Menu.");
                string viewType = Console.ReadLine();

                switch (viewType.ToLower())
                {
                    case "0":
                        Console.Clear();
                        return;

                    case "all":
                        Console.Clear();
                        tableCmd.CommandText = $"SELECT * FROM coding";
                        break;

                    case "day":
                        Console.WriteLine("Enter a day of the week: "); // write a method that ecexpts dayofweekInput as string and returns numeric value. Then use num value
                        var dayOfWeek = Console.ReadLine();
                        //var numbericDay = Helpers.GetNumericDay(dayOfWeek);
                                                                   
                        tableCmd.CommandText = $"SELECT * FROM coding WHERE DayOfWeek LIKE '{dayOfWeek}'"; 
                        break;

                    case "month":
                        Console.WriteLine("Enter the month in number format (eg. 05)");
                        var monthOfRecords = Console.ReadLine();
                        tableCmd.CommandText = $"SELECT * FROM coding WHERE SUBSTR(Date, 1, 2) = '{monthOfRecords}'";
                        break;

                    case "year":
                        Console.WriteLine("\n\nEnter the year of the records you want to view: (eg. 2023)");
                        var yearOfRecords = Console.ReadLine();
                        tableCmd.CommandText = $"SELECT * FROM coding WHERE SUBSTR(Date, 7, 4) = '{yearOfRecords}'";
                        break;

                    default:
                        Console.WriteLine("Invalid command. Review the options and try again.");
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
                                DayOfWeek = reader.GetString(2),
                                StartTime = TimeOnly.ParseExact(reader.GetString(3), "hh:mm tt", CultureInfo.InvariantCulture), // TODO change cultureinfo?
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

                connection.Close();

                ConsoleTableBuilder
                    .From(tableData)
                    .ExportAndWriteLine();

                Console.WriteLine($"Total duration spent coding: {summedDuration}");
                Console.WriteLine($"Average time spent per session: {averageDuration}");
            }
            Console.WriteLine($"Total Entries: {counter}");
            Console.WriteLine("=======================================");
            Console.WriteLine("Press Enter to Continue");
            Console.ReadLine();
        }
    }
}
