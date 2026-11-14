using SalesManagementSystem.Domain.Abstractions;

namespace SalesManagementSystem.Domain.Entities;

/// <summary>HR earns the base salary plus a fixed allowance.</summary>
public sealed class Hr : Employee, IApprovable, INotifiable
{
    private decimal _allowance;

    public decimal Allowance
    {
        get => _allowance;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Allowance cannot be negative.");
            _allowance = value;
        }
    }

    public Hr(string id, string name, decimal baseSalary, decimal allowance)
    {
        Id = id;
        Name = name;
        BaseSalary = baseSalary;
        Allowance = allowance;
    }

    public override decimal CalculateSalary() => BaseSalary + Allowance;

    public void Approve(string subject)
        => Console.WriteLine($"  [HR] approved: {subject}");

    public void SendNotification(string message)
        => Console.WriteLine($"  [HR] notification: {message}");
}
