using ATMConsoleApp.Models;

namespace ATMConsoleApp.DataAccess;

/// <summary>
/// Defines the contract for account data access operations.
/// </summary>
public interface IAccountRepository
{
    /// <summary>
    /// Retrieves an account by login and PIN.
    /// </summary>
    /// <param name="login">The username.</param>
    /// <param name="pin">The 5-digit PIN.</param>
    /// <returns>The matched account, or null.</returns>
    Account? GetAccountByLoginAndPin(string login, string pin);

    /// <summary>
    /// Retrieves an account by its ID.
    /// </summary>
    /// <param name="accountId">The account ID.</param>
    /// <returns>The matched account, or null.</returns>
    Account? GetAccountById(int accountId);

    /// <summary>
    /// Updates the balance of a specific account.
    /// </summary>
    /// <param name="accountId">The account ID.</param>
    /// <param name="newBalance">The new balance.</param>
    void UpdateAccountBalance(int accountId, decimal newBalance);
}
