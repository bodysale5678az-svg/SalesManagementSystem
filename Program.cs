using SalesManagementSystem.Application;
using SalesManagementSystem.Domain.Abstractions;
using SalesManagementSystem.Domain.Entities;
using SalesManagementSystem.Domain.Payments;
using SalesManagementSystem.Infrastructure.Repositories;

namespace SalesManagementSystem;

/// <summary>
/// Console demo that exercises every part of the design:
/// polymorphism, interfaces, repository swapping, order lifecycle
/// and payment strategies.
/// </summary>
internal static class Program
{
    private static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 70));
        Console.WriteLine($"  {title}");
        Console.WriteLine(new string('=', 70));
    }

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Sales Management System — OOP demo");
        Console.WriteLine("(.NET / C#)");

        DemonstratePolymorphism();
        DemonstrateInterfaces();
        DemonstrateRepository();
        DemonstrateOrders();
    }

    // ---------- 1. Polymorphism ----------
    private static void DemonstratePolymorphism()
    {
        Section("1. Payroll — polymorphism via an abstract base class");

        var staff = new List<Employee>
        {
            new Manager("M-01", "Sara Ahmed",   baseSalary: 12000m, bonus: 3000m),
            new Hr     ("H-02", "Khaled Omar",  baseSalary:  9000m, allowance: 1500m),
            new Developer("D-03", "Mona Saleh", baseSalary: 11000m, overtimeRate: 150m)
                { OvertimeHours = 12 },
            new SalesEmployee("S-04", "Ali Hassan", baseSalary: 8000m, commissionPercent: 15m)
        };

        decimal total = 0m;
        foreach (var person in staff)
        {
            Console.WriteLine("  " + person.Describe());
            total += person.CalculateSalary();
        }

        Console.WriteLine(new string('-', 70));
        Console.WriteLine($"  {"TOTAL PAYROLL",-58} {total,10:0.00}");
    }

    // ---------- 2. Interfaces ----------
    private static void DemonstrateInterfaces()
    {
        Section("2. Capability interfaces — IApprovable / IReportable / INotifiable");

        var everyone = new List<Employee>
        {
            new Manager("M-01", "Sara Ahmed", 12000m, 3000m),
            new Hr("H-02", "Khaled Omar", 9000m, 1500m),
            new Developer("D-03", "Mona Saleh", 11000m, 150m) { OvertimeHours = 5 },
            new SalesEmployee("S-04", "Ali Hassan", 8000m, 15m)
        };

        foreach (var person in everyone)
        {
            if (person is IApprovable approver)
                approver.Approve("annual leave request");

            if (person is IReportable reporter)
                Console.WriteLine($"  report: {reporter.Report()}");

            if (person is INotifiable notifier)
                notifier.SendNotification($"payroll for {person.Name} is ready");
        }
    }

    // ---------- 3. Repository ----------
    private static void DemonstrateRepository()
    {
        Section("3. Repository pattern — same service, two storage engines");

        RunRepositoryDemo(new InMemoryCustomerRepository(), "InMemoryCustomerRepository");
        RunRepositoryDemo(new ArrayCustomerRepository(), "ArrayCustomerRepository");
    }

    private static void RunRepositoryDemo(
        Domain.Repositories.ICustomerRepository repository, string label)
    {
        var service = new CustomerService(repository);

        service.Add(new Customer(1, "Ahmed Mostafa", "ahmed@example.com", "01000000001"));
        service.Add(new Customer(2, "Nour Adel", "nour@example.com", "01000000002"));
        service.Update(2, "Nour Adel", "nour.adel@example.com", "01111111111");

        Console.WriteLine($"  [{label}] customers:");
        foreach (var c in service.All())
            Console.WriteLine($"    {c}");

        service.Delete(1);
        Console.WriteLine($"  [{label}] after deleting #1: {service.All().Count} customer(s)");
    }

    // ---------- 4. Orders ----------
    private static void DemonstrateOrders()
    {
        Section("4. Order lifecycle — Pending → Confirmed → Paid");

        var laptop = new Product(1, "Laptop", 15000m, 10);
        var mouse = new Product(2, "Wireless Mouse", 350m, 50);
        var customer = new Customer(10, "Hassan Ali", "hassan@example.com", "01234567890");

        var order = new Order(1001, customer);
        order.AddItem(new OrderItem(laptop, 1));
        order.AddItem(new OrderItem(mouse, 2));

        Console.WriteLine($"  {order}");
        foreach (var item in order.Items)
            Console.WriteLine($"    {item}");

        order.Confirm();
        Console.WriteLine($"  after confirm → {order.Status}");

        order.PaymentMethod = new CreditCardPayment("4242");
        order.Pay();
        Console.WriteLine($"  after pay     → {order.Status}");
        Console.WriteLine($"  stock now: {laptop.Name}={laptop.StockQuantity}, {mouse.Name}={mouse.StockQuantity}");

        // lifecycle guard: paying twice must fail
        try
        {
            order.Pay();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  guarded: {ex.Message}");
        }

        Section("Done — all demonstrations completed");
    }
}
