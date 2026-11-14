using SalesManagementSystem.Domain.Entities;

namespace SalesManagementSystem.Domain.Repositories;

/// <summary>
/// Repository abstraction for customers.
/// The rest of the application depends on this interface only,
/// so the storage can be swapped (in-memory, SQL, API…) without
/// touching business code. This is the Dependency Inversion Principle.
/// </summary>
public interface ICustomerRepository
{
    bool Add(Customer customer);
    Customer? FindById(int customerId);
    bool Update(int id, string name, string email, string phone);
    IReadOnlyList<Customer> GetAll();
    bool Delete(int id);
}
