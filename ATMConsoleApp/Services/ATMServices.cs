using ATMConsoleApp.DataAccess;

namespace ATMConsoleApp.Services;

/// <summary>
/// Defines the contract for ATM business logic.
/// </summary>
public interface IAtmService
{
    /// <summary>
    /// Processes a cash withdrawal.
    /// </summary>
    /// <param name="accountId">The account ID.</param>
    /// <param name="amount">The amount to withdraw.</param>
    /// <param name="errorMessage">Output parameter containing the error message if the withdrawal fails.</param>
    /// <returns>True if successful, otherwise false.</returns>
    bool Withdraw(int accountId, decimal amount, out string errorMessage);
}

/// <summary>
/// Implements the ATM business logic and transaction validations.
/// </summary>
public class AtmService : IAtmService
{
    private readonly IAccountRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AtmService"/> class.
    /// </summary>
    /// <param name="repository">The injected account repository.</param>
    public AtmService(IAccountRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc/>
    public bool Withdraw(int accountId, decimal amount, out string errorMessage)
    {
        errorMessage = string.Empty;
        if (amount <= 0)
        {
            errorMessage = "Amount must be greater than zero.";
            return false;
        }

        var account = _repository.GetAccountById(accountId);
        if (account == null || account.Balance < amount)
        {
            errorMessage = "Insufficient funds.";
            return false;
        }

        _repository.UpdateAccountBalance(accountId, account.Balance - amount);
        return true;
    }
}
