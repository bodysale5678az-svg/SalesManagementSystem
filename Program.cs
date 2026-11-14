using SalesManagementSystem.Application;
using SalesManagementSystem.Domain.Abstractions;
using SalesManagementSystem.Domain.Entities;
using SalesManagementSystem.Domain.Payments;
using SalesManagementSystem.Domain.Repositories;
using SalesManagementSystem.Infrastructure.Persistence;
using SalesManagementSystem.Infrastructure.Repositories;

namespace SalesManagementSystem;

/// <summary>
/// Console demo that exercises every part of the design:
/// polymorphism, capability interfaces, the repository pattern,
/// the database aggregate root, and the order lifecycle.
/// </summary>
internal static class Program
{
    private static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 74));
        Console.WriteLine($"  {title}");
        Console.WriteLine(new string('=', 74));
    }

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Sales Management System — OOP demo (.NET / C#)");

        // One shared aggregate root for the whole run.
        var database = new Database();

        DemonstratePayroll(database);
        DemonstrateInterfaces(database);
        DemonstrateRepositoryPattern(database);
        DemonstrateOrderLifecycle(database);
        DemonstrateDatabaseSummary(database);
    }

    // ---------- 1. Polymorphism ----------
    private static void DemonstratePayroll(Database database)
    {
        Section("1. Payroll — polymorphism via an abstract base class");

        database.AddEmployee(new Manager("M-01", "Sara Ahmed", 12000m, 3000m));
        database.AddEmployee(new Hr("H-02", "Khaled Omar", 9000m, 1500m));
        database.AddEmployee(new Developer("D-03", "Mona Saleh", 11000m, 150m) { OvertimeHours = 12 });
        database.AddEmployee(new SalesEmployee("S-04", "Ali Hassan", 8000m, 15m));

        foreach (var person in database.Employees)
            Console.WriteLine("  " + person.Describe());

        Console.WriteLine(new string('-', 74));
        Console.WriteLine($"  {"TOTAL PAYROLL",-62} {database.TotalPayroll(),10:0.00}");
    }

    // ---------- 2. Interfaces ----------
    private static void DemonstrateInterfaces(Database database)
    {
        Section("2. Capability interfaces — IApprovable / IReportable / INotifiable");

        foreach (var person in database.Employees)
        {
            if (person is IApprovable approver)
                approver.Approve("annual leave request");

            if (person is IReportable reporter)
                Console.WriteLine($"  report: {reporter.Report()}");

            if (person is INotifiable notifier)
                notifier.SendNotification($"payroll for {person.Name} is ready");
        }
    }

    // ---------- 3. Repository pattern ----------
    private static void DemonstrateRepositoryPattern(Database database)
    {
        Section("3. Repository pattern — one service, two storages");

        RunRepositoryDemo(
            new DatabaseCustomerRepository(database),
            "DatabaseCustomerRepository (shared aggregate)",
            database);

        RunRepositoryDemo(
            new InMemoryCustomerRepository(),
            "InMemoryCustomerRepository (isolated dictionary)",
            database: null);
    }

    private static void RunRepositoryDemo(
        ICustomerRepository repository, string label, Database? database)
    {
        var service = new CustomerService(repository);

        service.Add(new Customer(1, "Ahmed Mostafa", "ahmed@example.com", "01000000001"));
        service.Add(new Customer(2, "Nour Adel", "nour@example.com", "01000000002"));
        service.Update(2, "Nour Adel", "nour.adel@example.com", "01111111111");

        Console.WriteLine($"  [{label}]");
        foreach (var customer in service.All())
            Console.WriteLine($"    {customer}");

        service.Delete(1);
        Console.WriteLine($"    after deleting #1: {service.All().Count} customer(s) remain");

        if (database is not null)
            Console.WriteLine($"    shared Database still holds {database.Customers.Count} customer(s)");
    }

    // ---------- 4. Order lifecycle ----------
    private static void DemonstrateOrderLifecycle(Database database)
    {
        Section("4. Order lifecycle — Pending → Confirmed → Paid");

        var laptop = new Product(1, "Laptop", 15000m, 10);
        var mouse = new Product(2, "Wireless Mouse", 350m, 50);
        database.AddProduct(laptop);
        database.AddProduct(mouse);

        var customer = new Customer(10, "Hassan Ali", "hassan@example.com", "01234567890");
        new DatabaseCustomerRepository(database).Add(customer);

        var order = new Order(1001, customer);
        order.AddItem(new OrderItem(laptop, 1));
        order.AddItem(new OrderItem(mouse, 2));
        database.AddOrder(order);

        Console.WriteLine($"  {order}");
        foreach (var item in order.Items)
            Console.WriteLine($"    {item}");

        order.Confirm();
        Console.WriteLine($"  after confirm → {order.Status}");

        order.PaymentMethod = new CreditCardPayment("4242");
        order.Pay();
        Console.WriteLine($"  after pay     → {order.Status}");
        Console.WriteLine($"  stock now: {laptop.Name}={laptop.StockQuantity}, {mouse.Name}={mouse.StockQuantity}");

        // Guard: illegal transitions must be rejected, not silently ignored.
        TryIllegal(() => order.Pay(), "pay twice");
        TryIllegal(() => order.Cancel(), "cancel a paid order");
        TryIllegal(() => new Product(3, "Bad", -5m, 0), "negative price");
    }

    private static void TryIllegal(Action action, string label)
    {
        try
        {
            action();
            Console.WriteLine($"  !! expected failure did not happen: {label}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  guarded ({label}): {ex.Message}");
        }
    }

    // ---------- 5. Aggregate root ----------
    private static void DemonstrateDatabaseSummary(Database database)
    {
        Section("5. Database aggregate root — composition over raw arrays");
        Console.WriteLine(database.Summary());

        Section("Done — all demonstrations completed");
    }
}
