# ATM System Architecture

## 1. Use Case Diagram

```mermaid
flowchart LR
    %% Actors
    Cust([👤 Customer])
    Admin([👤 Administrator])

    %% Use Cases
    subgraph ATM System
        Login(Log In)
        
        %% Customer Cases
        WC(Withdraw Cash)
        DC(Deposit Cash)
        DB(Display Balance)
        
        %% Admin Cases
        CNA(Create New Account)
        DEA(Delete Existing Account)
        UAI(Update Account Information)
        SFA(Search for Account)
        LO(Log Out)
    end

    %% Customer Connections
    Cust --> Login
    Cust --> WC
    Cust --> DC
    Cust --> DB
    Cust --> LO

    %% Admin Connections
    Admin --> Login
    Admin --> CNA
    Admin --> DEA
    Admin --> UAI
    Admin --> SFA
    Admin --> LO
```

## 2. System Architecture Diagram

```mermaid
flowchart TD
    %% User Interaction
    User((User))
    
    subgraph PresentationLayer [1. Presentation Layer]
        UI[ConsoleMenu.cs\nHandles I/O]
    end

    subgraph BusinessLogicLayer [2. Business Logic Layer]
        Service[AtmService.cs\nValidates Rules]
    end

    subgraph DataAccessLayer [3. Data Access Layer]
        Repo[MySqlAccountRepository.cs\nExecutes Queries]
    end

    subgraph Database
        DB[(MySQL Server\nATM_System)]
    end

    %% Connections
    User <-->|Reads/Types| UI
    UI <-->|Dependency Injection| Service
    Service <-->|Dependency Injection| Repo
    Repo <-->|SQL Queries / Port 3306| DB

    %% Styling
    classDef layer fill:#f9f9f9,stroke:#333,stroke-width:2px;
    class PresentationLayer,BusinessLogicLayer,DataAccessLayer layer;
```

## 3. Class Diagram

```mermaid
classDiagram
    %% Models
    class Account {
        +int AccountId
        +string Login
        +string PinCode
        +string Role
        +string HolderName
        +decimal Balance
        +string Status
        +UpdateBalance(decimal newBalance) void
    }

    %% Data Access Layer
    class IAccountRepository {
        <<interface>>
        +GetAccountByLoginAndPin(string login, string pin) Account?
        +GetAccountById(int accountId) Account?
        +UpdateAccountBalance(int accountId, decimal newBalance) void
    }

    class MySqlAccountRepository {
        -string _connectionString
        +MySqlAccountRepository(string connectionString)
        +GetAccountByLoginAndPin(string login, string pin) Account?
        +GetAccountById(int accountId) Account?
        +UpdateAccountBalance(int accountId, decimal newBalance) void
    }

    %% Business Logic Layer
    class IAtmService {
        <<interface>>
        +Withdraw(int accountId, decimal amount, out string errorMessage) bool
    }

    class AtmService {
        -IAccountRepository _repository
        +AtmService(IAccountRepository repository)
        +Withdraw(int accountId, decimal amount, out string errorMessage) bool
    }

    %% Presentation Layer
    class ConsoleMenu {
        -IAtmService _atmService
        -IAccountRepository _repository
        -Account? _currentUser
        +ConsoleMenu(IAtmService atmService, IAccountRepository repository)
        +Start() void
        -Login() bool
        -CustomerMenu() void
        -AdminMenu() void
        -HandleWithdrawal() void
        -DisplayBalance() void
        -HandleSearchAccount() void
    }

    class Program {
        <<internal>>
        ~Main(string[] args)$ void
    }

    %% Relationships (Dependency Injection & Implementation)
    IAccountRepository <|.. MySqlAccountRepository : Implements
    IAtmService <|.. AtmService : Implements
    
    AtmService --> IAccountRepository : Injects
    ConsoleMenu --> IAtmService : Injects
    ConsoleMenu --> IAccountRepository : Injects
    
    %% Data Flow Relationships
    MySqlAccountRepository ..> Account : Returns
    ConsoleMenu --> Account : Tracks Current User
    Program ..> ConsoleMenu : Bootstraps (DI Setup)
