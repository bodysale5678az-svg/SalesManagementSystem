using SalesManagementSystem.Domain.Abstractions;
using SalesManagementSystem.Domain.Entities;

namespace SalesManagementSystem.Infrastructure.Persistence;

/// <summary>
/// Database — an in-process "aggregate root" that owns every collection
/// of entities in the application.
///
/// This is the cleaned-up version of the original fixed-array idea:
/// instead of raw arrays with nullable slots, each collection is a
/// typed list behind a read-only projection, so callers can inspect
/// data but can only mutate it through the repositories/services.
///
/// Demonstrates COMPOSITION — the database *has* employees, customers,
/// products and orders; it does not inherit from them.
/// </summary>
public sealed class Database
{
    private readonly List<Employee> _employees = new();
    private readonly List<Customer> _customers = new();
    private readonly List<Product> _products = new();
    private readonly List<Order> _orders = new();

    public IReadOnlyList<Employee> Employees => _employees;
    public IReadOnlyList<Customer> Customers => _customers;
    public IReadOnlyList<Product> Products => _products;
    public IReadOnlyList<Order> Orders => _orders;

    // ---- employees ----
    public void AddEmployee(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);
        if (_employees.Any(e => e.Id == employee.Id))
            throw new InvalidOperationException($"Employee id '{employee.Id}' already exists.");
        _employees.Add(employee);
    }

    // ---- customers ----
    internal void AddCustomer(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        if (_customers.Any(c => c.Id == customer.Id))
            throw new InvalidOperationException($"Customer id '{customer.Id}' already exists.");
        _customers.Add(customer);
    }

    internal bool RemoveCustomer(int id) => _customers.RemoveAll(c => c.Id == id) > 0;

    // ---- products ----
    public void AddProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (_products.Any(p => p.Id == product.Id))
            throw new InvalidOperationException($"Product id '{product.Id}' already exists.");
        _products.Add(product);
    }

    public Product? FindProduct(int id) => _products.FirstOrDefault(p => p.Id == id);

    // ---- orders ----
    public void AddOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        if (_orders.Any(o => o.Id == order.Id))
            throw new InvalidOperationException($"Order id '{order.Id}' already exists.");
        _orders.Add(order);
    }

    // ---- reporting helpers ----
    public decimal TotalPayroll() => _employees.Sum(e => e.CalculateSalary());

    public decimal TotalRevenue()
        => _orders.Where(o => o.Status == OrderStatus.Paid).Sum(o => o.Total);

    public string Summary()
    {
        return string.Join(Environment.NewLine, new[]
        {
            $"  Employees : {_employees.Count}",
            $"  Customers : {_customers.Count}",
            $"  Products  : {_products.Count}",
            $"  Orders    : {_orders.Count}  (paid: {_orders.Count(o => o.Status == OrderStatus.Paid)})",
            $"  Payroll   : {TotalPayroll(),10:0.00}",
            $"  Revenue   : {TotalRevenue(),10:0.00}"
        });
    }
}
