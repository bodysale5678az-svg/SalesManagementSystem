using SalesManagementSystem.Domain.Entities;
using SalesManagementSystem.Domain.Repositories;

namespace SalesManagementSystem.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation backed by a simple dictionary.
/// Demonstrates that the business layer works against the
/// <see cref="ICustomerRepository"/> contract only.
/// </summary>
public sealed class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly Dictionary<int, Customer> _customers = new();

    public bool Add(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        return _customers.TryAdd(customer.Id, customer);
    }

    public Customer? FindById(int customerId)
        => _customers.TryGetValue(customerId, out var c) ? c : null;

    public bool Update(int id, string name, string email, string phone)
    {
        var customer = FindById(id);
        if (customer is null) return false;

        customer.Update(name, email, phone);
        return true;
    }

    public IReadOnlyList<Customer> GetAll() => _customers.Values.OrderBy(c => c.Id).ToList();

    public bool Delete(int id) => _customers.Remove(id);
}
