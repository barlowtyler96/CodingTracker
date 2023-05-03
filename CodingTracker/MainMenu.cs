﻿namespace CodingTracker
{
    internal class MainMenu
    {
        public static void GetUserInput()
        {
            Console.Clear();
            var closeApp = false;

            while (closeApp == false)
            {
                Console.WriteLine("\n\nCoding Tracker MAIN MENU");
                Console.WriteLine("\n====================================");
                Console.WriteLine("\n\nWhat would you like to do?");
                Console.WriteLine("Type 0 to exit");
                Console.WriteLine("Type 1 to View Records");
                Console.WriteLine("Type 2 to Update a Record");
                Console.WriteLine("Type 3 to Delete a Record");
                Console.WriteLine("Type 4 to Manually enter Coding Sessions");
                Console.WriteLine("Type 5 to Track a Coding Session via Stopwatch");
                Console.WriteLine("\n====================================");
                var commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        closeApp = true;
                        break;
                    case "1":
                        View.ViewRecords();
                        break;
                    case "2":
                        DbManager.UpdateRecord();
                        break;
                    case "3":
                        DbManager.DeleteRecord();
                        break;
                    case "4":
                        DbManager.ManualInsert();
                        break;
                    case "5":
                        //CRUD.StopwatchInsert();
                        break;
                }

            }
        }
    }
}