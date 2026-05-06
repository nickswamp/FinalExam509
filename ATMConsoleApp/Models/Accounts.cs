namespace ATMConsoleApp.Models;

/// <summary>
/// Represents a user account in the ATM system.
/// </summary>
public class Account
{
    /// <summary>
    /// Gets the unique account identifier.
    /// </summary>
    public int AccountId { get; init; }

    /// <summary>
    /// Gets the login username.
    /// </summary>
    public string Login { get; init; } = string.Empty;

    /// <summary>
    /// Gets the 5-digit pin code.
    /// </summary>
    public string PinCode { get; init; } = string.Empty;

    /// <summary>
    /// Gets the role of the user (Customer or Administrator).
    /// </summary>
    public string Role { get; init; } = string.Empty;

    /// <summary>
    /// Gets the full name of the account holder.
    /// </summary>
    public string HolderName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the current financial balance of the account.
    /// </summary>
    public decimal Balance { get; private set; }

    /// <summary>
    /// Gets the active or disabled status of the account.
    /// </summary>
    public string Status { get; init; } = "Active";

    /// <summary>
    /// Updates the balance of the account.
    /// </summary>
    /// <param name="newBalance">The new balance to apply.</param>
    public void UpdateBalance(decimal newBalance)
    {
        Balance = newBalance;
    }
}
