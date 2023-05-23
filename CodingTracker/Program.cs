using CodingTracker.View;
using System.Configuration;
namespace CodingTracker;

internal class Program
{
    private static readonly string connectionString = ConfigurationManager.AppSettings.Get("connString");
    static void Main(string[] args)
    {
        MainMenu.GetUserInput();
    }
}