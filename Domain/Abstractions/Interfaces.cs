namespace SalesManagementSystem.Domain.Abstractions;

/// <summary>Capable of approving requests (e.g. leave, orders, expenses).</summary>
public interface IApprovable
{
    void Approve(string subject);
}

/// <summary>Capable of producing a periodic report.</summary>
public interface IReportable
{
    string Report();
}

/// <summary>Capable of receiving notifications.</summary>
public interface INotifiable
{
    void SendNotification(string message);
}
