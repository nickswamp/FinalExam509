using System;
using ATMConsoleApp.DataAccess;
using ATMConsoleApp.Models;
using ATMConsoleApp.Services;

namespace ATMConsoleApp.UI;

/// <summary>
/// Handles all console-based user interaction and rendering.
/// </summary>
public class ConsoleMenu
{
    private readonly IAtmService _atmService;
    private readonly IAccountRepository _repository;
    private Account? _currentUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleMenu"/> class.
    /// </summary>
    /// <param name="atmService">The ATM business logic service.</param>
    /// <param name="repository">The account data repository.</param>
    public ConsoleMenu(IAtmService atmService, IAccountRepository repository)
    {
        _atmService = atmService;
        _repository = repository;
    }

    /// <summary>
    /// Starts the main application loop.
    /// </summary>
    public void Start()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the ATM System");

            if (Login())
            {
                if (_currentUser?.Role == "Customer")
                {
                    CustomerMenu();
                }
                else if (_currentUser?.Role == "Administrator")
                {
                    AdminMenu();
                }
            }
            else
            {
                Console.WriteLine("\nInvalid Login or PIN. Press any key to try again.");
                Console.ReadKey();
            }
        }
    }

    private bool Login()
    {
        Console.Write("Enter login: ");
        string login = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter Pin code: ");
        string pin = Console.ReadLine() ?? string.Empty;

        // Call the repository to check credentials
        _currentUser = _repository.GetAccountByLoginAndPin(login, pin);

        return _currentUser != null;
    }

    private void CustomerMenu()
    {
        bool loggedIn = true;
        while (loggedIn)
        {
            Console.Clear();
            Console.WriteLine($"Customer Menu - Welcome {_currentUser?.HolderName}");
            Console.WriteLine("1 - Withdraw Cash");
            Console.WriteLine("2 - Deposit Cash");
            Console.WriteLine("3 - Display Balance");
            Console.WriteLine("4 - Exit");
            Console.Write("Selection: ");

            string choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    HandleWithdrawal();
                    break;
                case "2":
                    // You will implement HandleDeposit similar to HandleWithdrawal
                    Console.WriteLine("Deposit feature pending migration...");
                    break;
                case "3":
                    DisplayBalance();
                    break;
                case "4":
                    loggedIn = false;
                    _currentUser = null;
                    break;
                default:
                    Console.WriteLine("Invalid selection.");
                    break;
            }

            if (loggedIn)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private void HandleWithdrawal()
    {
        Console.Write("\nEnter the withdrawal amount: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            // The UI passes the request to the Business Logic layer (AtmService)
            bool success = _atmService.Withdraw(_currentUser!.AccountId, amount, out string errorMessage);

            if (success)
            {
                // Fetch updated account to show new balance
                _currentUser = _repository.GetAccountById(_currentUser.AccountId);
                Console.WriteLine("\nCash Successfully Withdrawn");
                Console.WriteLine($"Account #{_currentUser?.AccountId}");
                Console.WriteLine($"Date: {DateTime.Now:MM/dd/yyyy}");
                Console.WriteLine($"Withdrawn: {amount:C}");
                Console.WriteLine($"Balance: {_currentUser?.Balance:C}");
            }
            else
            {
                Console.WriteLine($"\nError: {errorMessage}");
            }
        }
        else
        {
            Console.WriteLine("Invalid amount format.");
        }
    }

    private void DisplayBalance()
    {
        // Always fetch fresh data before displaying
        _currentUser = _repository.GetAccountById(_currentUser!.AccountId);
        Console.WriteLine($"\nAccount #{_currentUser?.AccountId}");
        Console.WriteLine($"Date: {DateTime.Now:MM/dd/yyyy}");
        Console.WriteLine($"Balance: {_currentUser?.Balance:C}");
    }

    private void AdminMenu()
    {
        bool loggedIn = true;
        while (loggedIn)
        {
            Console.Clear();
            Console.WriteLine("=== Administrator Menu ===");
            Console.WriteLine("1 - Create New Account");
            Console.WriteLine("2 - Delete Existing Account");
            Console.WriteLine("3 - Update Account Information");
            Console.WriteLine("4 - Search for Account");
            Console.WriteLine("5 - Exit");
            Console.Write("Selection: ");

            string choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "4":
                    HandleSearchAccount();
                    break;
                case "5":
                    loggedIn = false;
                    _currentUser = null;
                    break;
                default:
                    Console.WriteLine("Feature pending migration...");
                    break;
            }

            if (loggedIn)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private void HandleSearchAccount()
    {
        Console.Write("\nEnter Account number: ");
        if (int.TryParse(Console.ReadLine(), out int searchId))
        {
            var account = _repository.GetAccountById(searchId);
            if (account != null && account.Role == "Customer")
            {
                Console.WriteLine("\nThe account information is:");
                Console.WriteLine($"Account # {account.AccountId}");
                Console.WriteLine($"Holder: {account.HolderName}");
                Console.WriteLine($"Balance: {account.Balance:C}");
                Console.WriteLine($"Status: {account.Status}");
                Console.WriteLine($"Login: {account.Login}");
                Console.WriteLine($"Pin Code: {account.PinCode}");
            }
            else
            {
                Console.WriteLine("Error: Customer account not found.");
            }
        }
    }
}
