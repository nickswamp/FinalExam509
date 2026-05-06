using System;
using ATMConsoleApp.Models;
using MySql.Data.MySqlClient;

namespace ATMConsoleApp.DataAccess;

/// <summary>
/// Provides data access operations for ATM Accounts using a MySQL database.
/// </summary>
public class MySqlAccountRepository : IAccountRepository
{
    private readonly string _connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlAccountRepository"/> class.
    /// </summary>
    /// <param name="connectionString">The MySQL database connection string.</param>
    public MySqlAccountRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Retrieves an account by its unique ID.
    /// </summary>
    /// <param name="accountId">The unique identifier for the account.</param>
    /// <returns>An <see cref="Account"/> object if found; otherwise, null.</returns>
    public Account? GetAccountById(int accountId)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string query = "SELECT AccountID, Login, PinCode, Role, HolderName, Balance, Status FROM Accounts WHERE AccountID = @id";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@id", accountId);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            var account = new Account
            {
                AccountId = reader.GetInt32("AccountID"),
                Login = reader.GetString("Login"),
                PinCode = reader.GetString("PinCode"),
                Role = reader.GetString("Role"),
                HolderName = reader.GetString("HolderName"),
                Status = reader.GetString("Status")
            };
            account.UpdateBalance(reader.GetDecimal("Balance"));
            return account;
        }

        return null;
    }

    /// <summary>
    /// Authenticates a user and retrieves their account based on login and PIN.
    /// </summary>
    /// <param name="login">The user's login username.</param>
    /// <param name="pin">The user's 5-digit PIN.</param>
    /// <returns>An <see cref="Account"/> object if credentials match; otherwise, null.</returns>
    public Account? GetAccountByLoginAndPin(string login, string pin)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string query = "SELECT AccountID, Login, PinCode, Role, HolderName, Balance, Status FROM Accounts WHERE Login = @login AND PinCode = @pin AND Status = 'Active'";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@login", login);
        cmd.Parameters.AddWithValue("@pin", pin);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            var account = new Account
            {
                AccountId = reader.GetInt32("AccountID"),
                Login = reader.GetString("Login"),
                PinCode = reader.GetString("PinCode"),
                Role = reader.GetString("Role"),
                HolderName = reader.GetString("HolderName"),
                Status = reader.GetString("Status")
            };
            account.UpdateBalance(reader.GetDecimal("Balance"));
            return account;
        }

        return null;
    }

    /// <summary>
    /// Updates the financial balance of a specific account in the database.
    /// </summary>
    /// <param name="accountId">The unique identifier of the account to update.</param>
    /// <param name="newBalance">The new balance to set.</param>
    public void UpdateAccountBalance(int accountId, decimal newBalance)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string query = "UPDATE Accounts SET Balance = @balance WHERE AccountID = @id";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@balance", newBalance);
        cmd.Parameters.AddWithValue("@id", accountId);

        cmd.ExecuteNonQuery();
    }
}
