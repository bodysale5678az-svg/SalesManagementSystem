namespace SalesManagementSystem.Domain.Entities;

/// <summary>
/// A customer of the store.
/// Note: the original draft had a self-referencing Phone setter
/// (`Phone = value` inside the Phone setter) which caused infinite
/// recursion — it now stores into a backing field.
/// </summary>
public sealed class Customer
{
    private string _name = string.Empty;
    private string _email = string.Empty;
    private string _phone = string.Empty;

    public int Id { get; }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Customer name cannot be empty.", nameof(value));
            _name = value.Trim();
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
                throw new ArgumentException("Email must be a valid address.", nameof(value));
            _email = value.Trim();
        }
    }

    public string Phone
    {
        get => _phone;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Phone cannot be empty.", nameof(value));
            _phone = value.Trim();
        }
    }

    public Customer(int id, string name, string email, string phone)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Customer id must be positive.");

        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
    }

    public void Update(string name, string email, string phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
    }

    public override string ToString() => $"#{Id} {Name} <{Email}> {Phone}";
}
