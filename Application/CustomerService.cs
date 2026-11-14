using SalesManagementSystem.Domain.Entities;
using SalesManagementSystem.Domain.Repositories;

namespace SalesManagementSystem.Application;

/// <summary>
/// Application service for customer operations.
/// Business rules live here; storage details stay behind the repository.
/// </summary>
public sealed class CustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
        => _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public bool Add(Customer customer) => _repository.Add(customer);

    public Customer? FindById(int id) => _repository.FindById(id);

    public bool Update(int id, string name, string email, string phone)
        => _repository.Update(id, name, email, phone);

    public bool Delete(int id) => _repository.Delete(id);

    public IReadOnlyList<Customer> All() => _repository.GetAll();
}
