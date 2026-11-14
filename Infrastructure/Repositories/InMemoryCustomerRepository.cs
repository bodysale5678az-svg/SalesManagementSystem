using SalesManagementSystem.Domain.Entities;
using SalesManagementSystem.Domain.Repositories;

namespace SalesManagementSystem.Infrastructure.Repositories;

/// <summary>
/// Dictionary-backed, standalone implementation of
/// <see cref="ICustomerRepository"/>. Unlike
/// <c>DatabaseCustomerRepository</c>, this one keeps its own storage,
/// which makes it ideal for unit tests and for demonstrating that the
/// application layer cannot tell the two apart.
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
        => _customers.TryGetValue(customerId, out var customer) ? customer : null;

    public bool Update(int id, string name, string email, string phone)
    {
        var customer = FindById(id);
        if (customer is null) return false;

        customer.Update(name, email, phone);
        return true;
    }

    public IReadOnlyList<Customer> GetAll()
        => _customers.Values.OrderBy(c => c.Id).ToList();

    public bool Delete(int id) => _customers.Remove(id);
}
