using SalesManagementSystem.Domain.Abstractions;

namespace SalesManagementSystem.Domain.Entities;

/// <summary>A developer earns the base salary plus overtime pay.</summary>
public sealed class Developer : Employee, IApprovable, IReportable
{
    private decimal _overtimeRate;

    /// <summary>Pay per overtime hour.</summary>
    public decimal OvertimeRate
    {
        get => _overtimeRate;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Overtime rate cannot be negative.");
            _overtimeRate = value;
        }
    }

    public int OvertimeHours { get; set; }

    public Developer(string id, string name, decimal baseSalary, decimal overtimeRate)
    {
        Id = id;
        Name = name;
        BaseSalary = baseSalary;
        OvertimeRate = overtimeRate;
    }

    public override decimal CalculateSalary() => BaseSalary + (OvertimeRate * OvertimeHours);

    public void Approve(string subject)
        => Console.WriteLine($"  [Developer] approved: {subject}");

    public string Report()
        => $"Developer {Name}: {OvertimeHours}h overtime at {OvertimeRate:0.00}/h";
}
