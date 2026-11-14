# 💼 Sales Management System — C# OOP

![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8%2B-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![CI](https://github.com/bodysale5678az-svg/SalesManagementSystem/actions/workflows/ci.yml/badge.svg)

A business domain model written in C# to practise **object-oriented design** properly:
abstraction, inheritance, polymorphism, interfaces, the **Repository pattern** and the **Strategy pattern**.

---

## 📸 Demo output

```
======================================================================
  1. Payroll — polymorphism via an abstract base class
======================================================================
  Manager         | M-01 | Sara Ahmed   | base 12000.00 | total 15000.00
  Hr              | H-02 | Khaled Omar  | base  9000.00 | total 10500.00
  Developer       | D-03 | Mona Saleh   | base 11000.00 | total 12800.00
  SalesEmployee   | S-04 | Ali Hassan   | base  8000.00 | total  9200.00
----------------------------------------------------------------------
  TOTAL PAYROLL                                             47500.00
```

---

## 🏗️ Architecture

```
SalesManagementSystem/
├── Domain/
│   ├── Abstractions/     Employee (abstract), IApprovable,
│   │                     IReportable, INotifiable
│   ├── Entities/         Customer, Product, Order, OrderItem,
│   │                     Manager, Hr, Developer, SalesEmployee
│   ├── Payments/         IPaymentMethod + Cash / CreditCard / BankTransfer
│   └── Repositories/     ICustomerRepository
├── Application/          CustomerService (use cases)
├── Infrastructure/
│   ├── Persistence/      Database (aggregate root)
│   └── Repositories/     DatabaseCustomerRepository,
│                         InMemoryCustomerRepository
└── Program.cs            demo runner
```

### Dependency direction

```
Program ──▶ Application ──▶ Domain
                  ▲
                  │
           Infrastructure  (implements Domain interfaces)
```

`Domain` knows nothing about storage. `Application` depends only on
`ICustomerRepository`. `Infrastructure` plugs into it. That is the
**Dependency Inversion Principle** in practice.

### The `Database` aggregate root

The original draft kept four fixed arrays (`Employees[20]`,
`Customers[30]`, `Products[60]`, `Orders[50]`) with nullable slots.
That version is replaced by a `Database` class that **owns typed
collections through composition** and exposes read-only projections:

```csharp
public IReadOnlyList<Employee> Employees => _employees;
public decimal TotalPayroll() => _employees.Sum(e => e.CalculateSalary());
public decimal TotalRevenue() => _orders.Where(o => o.Status == OrderStatus.Paid).Sum(o => o.Total);
```

Callers can read everything but can only mutate through the
repositories — so business invariants cannot be bypassed.

---

## 🎯 OOP concepts demonstrated

| Concept | Where |
|---|---|
| **Abstraction** | `abstract class Employee` with `abstract CalculateSalary()` |
| **Inheritance** | `Manager`, `Hr`, `Developer`, `SalesEmployee` : `Employee` |
| **Polymorphism** | One `List<Employee>` — each instance computes its own salary |
| **Encapsulation** | Private backing fields + validated setters that throw |
| **Interfaces** | `IApprovable`, `IReportable`, `INotifiable`, `ICustomerRepository`, `IPaymentMethod` |
| **Interface segregation** | A role implements only the capabilities it actually has |
| **Repository pattern** | Swap `DatabaseCustomerRepository` ⇄ `InMemoryCustomerRepository` without touching services |
| **Strategy pattern** | `IPaymentMethod` — cash / credit card / bank transfer |
| **Composition** | `Database` *has* employees, customers, products and orders |
| **Aggregate root** | `Database` is the single entry point to persisted collections |
| **State machine** | `Order`: Pending → Confirmed → Paid, with illegal transitions rejected |
| **Null safety** | `Customer?` / `Product?` with `ThrowIfNull` |

---

## 🐛 Bugs fixed from the original draft

The starting point was an early student version. These issues were fixed:

| Issue | Fix |
|---|---|
| **`Customer.Phone` setter assigned `Phone = value`** (infinite recursion → StackOverflow) | Writes to a private backing field |
| **`Product.Price` / `StockQuantity` getters returned themselves** (same recursion bug, found in the other draft) | Backing fields |
| **`OrderItem` computed `UnitPrice = quantity * price`** then multiplied by quantity again — every invoice was wrong by a factor of the quantity | `LineTotal => Price * Quantity` |
| `manager.cs` used `this.Id` while the base class defined `ID` | Unified to `Id` |
| `DEVELOPERMANAGEMENT.cs` had an extra `}` — project did not compile | Corrected structure |
| No `Program.cs` — nothing could run | Added a full demo runner |
| Classes nested inside empty wrapper classes for no reason | Flattened into namespaces |
| Errors printed with `Console.WriteLine("error")` | Replaced with real exceptions |
| Misleading file names (`PROUDCT.cs`, `CUSTOMETR.cs`, `Orderiteam.cs`) | Renamed to the real type names |
| Two identical repositories, one dishonestly named `SQLCUSTOMERMANAGEMENT` | One honest `DatabaseCustomerRepository` + one isolated `InMemoryCustomerRepository` |
| Fixed-size arrays with nullable slots | `Database` aggregate root with typed collections |

---

## 🚀 Run

```bash
dotnet run
```

Requirements: .NET 8 SDK or newer.

---

## 🗺️ Roadmap

- [ ] Unit tests (xUnit) for salary rules and the order lifecycle
- [ ] Replace in-memory storage with EF Core + SQL Server
- [ ] Extract payroll calculation into a dedicated `PayrollService`
- [ ] Add pagination to `ICustomerRepository.GetAll`

---

## 👤 Author

**Mahmoud Saleh** — [@bodysale5678az-svg](https://github.com/bodysale5678az-svg)
