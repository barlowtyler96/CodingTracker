﻿using CodingTracker.View;
using System.Globalization;
namespace CodingTracker.Helpers;

internal class Helper
{
    internal static string GetDateInput(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        var dateInput = Console.ReadLine().Trim();

        if (dateInput == "0")
            MainMenu.GetUserInput();

        while (!DateTime.TryParseExact(dateInput, "MM-dd-yyyy", new CultureInfo("en-us"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid date. Enter a date in the following format: (mm-dd-yyyy)" +
                              " or type 0 to return to the Main Menu.");
            dateInput = Console.ReadLine();
        }
        return dateInput;
    }

    internal static string GetDayInput(string message)
    {
        Console.WriteLine(message);
        var dayInput = Console.ReadLine();

        if (dayInput == "0")
            MainMenu.GetUserInput();

        while (!DateTime.TryParseExact(dayInput, "dddd", new CultureInfo("en-us"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid day. Enter a valid day (eg. Monday, Tuesday)");
            dayInput = Console.ReadLine();
        }
        return dayInput;
    }

    internal static string GetHourTime(string message)
    {
        Console.WriteLine(message);
        var hourTime = Console.ReadLine();

        if (hourTime == "0")
            MainMenu.GetUserInput();

        while (!DateTime.TryParseExact(hourTime, "hh:mm tt", new CultureInfo("en-us"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid time. Enter the time in the following format: (01:00 PM)" +
                              " or type 0 to return to the Main Menu.");
            hourTime = Console.ReadLine();
        }

        return hourTime;
    }

    internal static string CalculateDuration(string startTime, string endTime)
    {
        var parsedStartTime = DateTime.ParseExact(startTime, "h:mm tt", new CultureInfo("en-US"));

        var parsedEndTime = DateTime.ParseExact(endTime, "h:mm tt", new CultureInfo("en-US"));


        var duration = parsedEndTime - parsedStartTime;

        return duration.ToString();
    }

    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        var numberInput = Console.ReadLine().Trim();

        if (numberInput == "0")
            MainMenu.GetUserInput();

        while (int.TryParse(numberInput, out _) == false || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("Invalid Id. Try again.");
            numberInput = Console.ReadLine();
        }

        var finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }

    internal static string GetViewType()
    {
        Console.WriteLine("Enter 'all' to view all records, 'day' to view by day" +
                          "'week' to view by week, or 'year' to view by year. Type 0 to " +
                          "return to the Main Menu.");
        var viewType = Console.ReadLine().Trim();

        if (viewType == "0")
            MainMenu.GetUserInput();

        return viewType;
    }
}