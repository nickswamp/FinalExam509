using Xunit;
using Moq;
using ATMConsoleApp.Services;
using ATMConsoleApp.DataAccess;
using ATMConsoleApp.Models;

namespace ATMConsoleApp.Tests;

public class AtmServiceTests
{
    private readonly Mock<IAccountRepository> _mockRepo;
    private readonly AtmService _atmService;

    public AtmServiceTests()
    {
        // This runs before every test, giving us a fresh mock database
        _mockRepo = new Mock<IAccountRepository>();
        _atmService = new AtmService(_mockRepo.Object);
    }

    [Fact]
    public void Withdraw_AmountIsZeroOrNegative_ReturnsFalse()
    {
        // Act
        bool result = _atmService.Withdraw(1, -50.0m, out string error);

        // Assert
        Assert.False(result);
        Assert.Equal("Amount must be greater than zero.", error);

        // Verify the database was never touched
        _mockRepo.Verify(r => r.UpdateAccountBalance(It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public void Withdraw_AccountDoesNotExist_ReturnsFalse()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetAccountById(99)).Returns((Account?)null);

        // Act
        bool result = _atmService.Withdraw(99, 100.0m, out string error);

        // Assert
        Assert.False(result);
        Assert.Equal("Insufficient funds.", error); // Based on our current logic, null falls into this block
    }

    [Fact]
    public void Withdraw_InsufficientFunds_ReturnsFalse()
    {
        // Arrange
        var poorAccount = new Account { AccountId = 1 };
        poorAccount.UpdateBalance(50.0m); // Only $50 in the bank
        _mockRepo.Setup(r => r.GetAccountById(1)).Returns(poorAccount);

        // Act
        bool result = _atmService.Withdraw(1, 100.0m, out string error);

        // Assert
        Assert.False(result);
        Assert.Equal("Insufficient funds.", error);
        _mockRepo.Verify(r => r.UpdateAccountBalance(It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public void Withdraw_ValidTransaction_ReturnsTrueAndUpdatesDatabase()
    {
        // Arrange
        var richAccount = new Account { AccountId = 2 };
        richAccount.UpdateBalance(500.0m); // $500 in the bank
        _mockRepo.Setup(r => r.GetAccountById(2)).Returns(richAccount);

        // Act
        bool result = _atmService.Withdraw(2, 100.0m, out string error);

        // Assert
        Assert.True(result);
        Assert.Empty(error);

        // Verify the database was told to update the balance to $400
        _mockRepo.Verify(r => r.UpdateAccountBalance(2, 400.0m), Times.Once);
    }
}
