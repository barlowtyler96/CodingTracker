using Microsoft.Data.Sqlite;

namespace CodingTracker
{
    internal class DbManager
    {
        internal static void ManualInsert()
        {
            string date = Helpers.GetDateInput("Enter the date for the record you are recording: (eg. mm-dd-yyyy)");

            string startTime = Helpers.GetHourTime("Enter the Start time for the session: (eg. 01:00 AM)" +
                                                    "or type 0 to return to the Main Menu.");// pass date as an argument for retreiving the start date/time
            string endTime = Helpers.GetHourTime("Enter the End time for the session: (eg. 01:00 AM)" +
                                                 "or type 0 to return to the Main Menu.");
            string duration = Helpers.CalculateDuration(startTime, endTime);

            Console.WriteLine(duration);

            using (var connection = new SqliteConnection(Program.ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"INSERT INTO coding(Date, StartTime, EndTime, Duration) VALUES ('{date}', '{startTime}', '{endTime}', '{duration}')";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
            //Console.Clear();
        }

        internal static void UpdateRecord()
        {
            Console.Clear();

            //View.ViewRecords(); // TODO Display all records with ConsoleTableExt

            var recordId = Helpers.GetNumberInput("\n\nPlease type the Id of the record you'd like to update " +
                                                "or 0 to return to the Main Menu");
            using (var connection = new SqliteConnection(Program.ConnectionString))
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

                string startTime = Helpers.GetHourTime("Enter the Start time for the session: (eg. 01:00 AM)" +
                                        "or type 0 to return to the Main Menu.");// pass date as an argument for retreiving the start date/time

                string endTime = Helpers.GetHourTime("Enter the End time for the session: (eg. 01:00 AM)" +
                                                     "or type 0 to return to the Main Menu.");

                string duration = Helpers.CalculateDuration(startTime, endTime);


                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE coding SET date = '{date}', StartTime = '{startTime}', EndTime = '{endTime}', Duration = {duration}" +
                                       $" WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal static void DeleteRecord()
        {
            Console.Clear();
            //View.ViewRecords(); // TODO Display all records with ConsoleTableExt

            var recordId = Helpers.GetNumberInput("\n\nPlease type the Id of the record you'd like to delete " +
                                                "or 0 to return to the Main Menu");

            using (var connection = new SqliteConnection(Program.ConnectionString))
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
    }
}