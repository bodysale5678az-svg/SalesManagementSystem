using SalesManagementSystem.Domain.Abstractions;

namespace SalesManagementSystem.Domain.Entities;

/// <summary>Sales employee earns the base salary plus a commission.</summary>
public sealed class SalesEmployee : Employee, INotifiable
{
    private decimal _commission;

    /// <summary>Commission as a percentage of the base salary (0–100).</summary>
    public decimal CommissionPercent
    {
        get => _commission;
        set
        {
            if (value < 0 || value > 100)
                throw new ArgumentOutOfRangeException(nameof(value), "Commission must be between 0 and 100.");
            _commission = value;
        }
    }

    public SalesEmployee(string id, string name, decimal baseSalary, decimal commissionPercent)
    {
        Id = id;
        Name = name;
        BaseSalary = baseSalary;
        CommissionPercent = commissionPercent;
    }

    public override decimal CalculateSalary() => BaseSalary + (BaseSalary * CommissionPercent / 100m);

    public void SendNotification(string message)
        => Console.WriteLine($"  [Sales] notification: {message}");
}
