namespace SalesManagementSystem.Domain.Abstractions;

/// <summary>
/// Base type for every person employed by the company.
/// Holds the identity and salary data shared by all roles and
/// forces every concrete employee to define how it is paid.
/// </summary>
public abstract class Employee
{
    private string _id = string.Empty;
    private string _name = string.Empty;
    private decimal _baseSalary;

    /// <summary>Unique employee identifier. Cannot be blank.</summary>
    public string Id
    {
        get => _id;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Employee id cannot be empty.", nameof(value));
            _id = value.Trim();
        }
    }

    /// <summary>Full name. Cannot be blank.</summary>
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Employee name cannot be empty.", nameof(value));
            _name = value.Trim();
        }
    }

    /// <summary>Monthly base salary. Cannot be negative.</summary>
    public decimal BaseSalary
    {
        get => _baseSalary;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Base salary cannot be negative.");
            _baseSalary = value;
        }
    }

    /// <summary>Total pay for the period, computed by each concrete role.</summary>
    public abstract decimal CalculateSalary();

    public virtual string Describe()
        => $"{GetType().Name,-15} | {Id,-6} | {Name,-18} | base {BaseSalary,10:0.00} | total {CalculateSalary(),10:0.00}";

    public override string ToString() => Describe();
}
