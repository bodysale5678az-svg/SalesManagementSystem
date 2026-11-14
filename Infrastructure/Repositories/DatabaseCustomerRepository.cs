using SalesManagementSystem.Domain.Entities;
using SalesManagementSystem.Domain.Repositories;
using SalesManagementSystem.Infrastructure.Persistence;

namespace SalesManagementSystem.Infrastructure.Repositories;

/// <summary>
/// Array-backed customer repository built on top of the shared
/// <see cref="Database"/> aggregate root.
///
/// The original draft had two classes with nearly identical bodies
/// (`InMemoryCustomerRepository` and `SQLCUSTOMERMANAGEMENT`). This
/// version keeps a single honest name — the storage is in-process.
/// </summary>
public sealed class DatabaseCustomerRepository : ICustomerRepository
{
    private readonly Database _database;

    public DatabaseCustomerRepository(Database database)
        => _database = database ?? throw new ArgumentNullException(nameof(database));

    public bool Add(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        if (FindById(customer.Id) is not null) return false;

        _database.AddCustomer(customer);
        return true;
    }

    public Customer? FindById(int customerId)
        => _database.Customers.FirstOrDefault(c => c.Id == customerId);

    public bool Update(int id, string name, string email, string phone)
    {
        var customer = FindById(id);
        if (customer is null) return false;

        customer.Update(name, email, phone);
        return true;
    }

    public IReadOnlyList<Customer> GetAll()
        => _database.Customers.OrderBy(c => c.Id).ToList();

    public bool Delete(int id) => _database.RemoveCustomer(id);
}
