using SalesManagementSystem.Domain.Abstractions;

namespace SalesManagementSystem.Domain.Entities;

/// <summary>Manager earns the base salary plus a fixed bonus.</summary>
public sealed class Manager : Employee, IApprovable, IReportable
{
    private decimal _bonus;

    public decimal Bonus
    {
        get => _bonus;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Bonus cannot be negative.");
            _bonus = value;
        }
    }

    public Manager(string id, string name, decimal baseSalary, decimal bonus)
    {
        Id = id;
        Name = name;
        BaseSalary = baseSalary;
        Bonus = bonus;
    }

    public override decimal CalculateSalary() => BaseSalary + Bonus;

    public void Approve(string subject)
        => Console.WriteLine($"  [Manager] approved: {subject}");

    public string Report()
        => $"Manager {Name}: team payroll contribution {CalculateSalary():0.00}";
}
