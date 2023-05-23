using CodingTracker.Controller;
using System.Configuration;
namespace CodingTracker.View;

internal class MainMenu
{
    private static readonly string connectionString = ConfigurationManager.AppSettings.Get("connString");
    public static void GetUserInput()
    {
        var closeApp = false;

        while (closeApp == false)
        {
            Console.Clear();
            Console.WriteLine("\n\nCoding Tracker MAIN MENU");
            Console.WriteLine("\n=======================================");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("Type 0 to exit");
            Console.WriteLine("Type 1 to View Records");
            Console.WriteLine("Type 2 to Update a Record");
            Console.WriteLine("Type 3 to Delete a Record");
            Console.WriteLine("Type 4 to Manually log Coding Sessions");
            Console.WriteLine("Type 5 to Track Session via Stopwatch");
            Console.WriteLine("\n=======================================");
            var commandInput = Console.ReadLine();

            switch (commandInput)
            {
                case "0":
                    Console.WriteLine("Goodbye!");
                    closeApp = true;
                    break;
                case "1":
                    DbController.ViewRecords();
                    break;
                case "2":
                    DbController.UpdateRecord();
                    break;
                case "3":
                    DbController.DeleteRecord();
                    break;
                case "4":
                    DbController.ManualInsert();
                    break;
                case "5":
                    DbController.StopwatchInsert();
                    break;
            }
        }
    }
}
