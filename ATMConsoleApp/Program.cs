using System;
using ATMConsoleApp.DataAccess;
using ATMConsoleApp.Services;
using ATMConsoleApp.UI;
using Microsoft.Extensions.DependencyInjection;

namespace ATMConsoleApp;

/// <summary>
/// The main entry point class for the ATM application.
/// </summary>
internal class Program
{
    /// <summary>
    /// The main entry point method.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    internal static void Main(string[] args)
    {
        try
        {
            // 1. Configure Services (The DI Container)
            var serviceProvider = new ServiceCollection()
                .AddTransient<IAccountRepository>(provider => new MySqlAccountRepository("Server=localhost;Database=ATM_System;Uid=root;Pwd=password;"))
                .AddTransient<IAtmService, AtmService>()
                .AddTransient<ConsoleMenu>()
                .BuildServiceProvider();

            // 2. Resolve the UI and start the app
            var menu = serviceProvider.GetService<ConsoleMenu>();
            menu?.Start();
        }
        catch (Exception ex)
        {
            // Global Exception Handler
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("=== CRITICAL SYSTEM FAILURE ===");
            Console.WriteLine("The ATM encountered an unexpected error and must shut down.");
            Console.WriteLine($"\nError Details: {ex.Message}");
            Console.ResetColor();
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}
