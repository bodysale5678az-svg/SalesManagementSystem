using SalesManagementSystem.Domain.Entities;
using SalesManagementSystem.Domain.Repositories;

namespace SalesManagementSystem.Infrastructure.Repositories;

/// <summary>
/// Second implementation of the same contract — fixed-capacity array
/// storage, mirroring the original assignment's "database" idea.
/// Shows why programming to an interface pays off: the caller cannot
/// tell which implementation it is using.
/// </summary>
public sealed class ArrayCustomerRepository : ICustomerRepository
{
    private readonly Customer?[] _slots;

    public ArrayCustomerRepository(int capacity = 30)
    {
        if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        _slots = new Customer?[capacity];
    }

    public bool Add(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        if (FindById(customer.Id) is not null) return false;

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] is null)
            {
                _slots[i] = customer;
                return true;
            }
        }
        return false; // full
    }

    public Customer? FindById(int customerId)
    {
        foreach (var c in _slots)
            if (c is not null && c.Id == customerId)
                return c;
        return null;
    }

    public bool Update(int id, string name, string email, string phone)
    {
        var customer = FindById(id);
        if (customer is null) return false;

        customer.Update(name, email, phone);
        return true;
    }

    public IReadOnlyList<Customer> GetAll()
        => _slots.Where(c => c is not null).Cast<Customer>().OrderBy(c => c.Id).ToList();

    public bool Delete(int id)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] is not null && _slots[i]!.Id == id)
            {
                _slots[i] = null;
                return true;
            }
        }
        return false;
    }
}
