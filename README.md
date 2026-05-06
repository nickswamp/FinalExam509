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

    flowchart TD
    %% User Interaction
    User((User))
    
    subgraph Presentation Layer [1. Presentation Layer]
        UI[ConsoleMenu.cs\nHandles I/O]
    end

    subgraph Business Logic Layer [2. Business Logic Layer]
        Service[AtmService.cs\nValidates Rules]
    end

    subgraph Data Access Layer [3. Data Access Layer]
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
    class Presentation Layer,Business Logic Layer,Data Access Layer layer;
