using SalesManagementSystem.Domain.Payments;

namespace SalesManagementSystem.Domain.Entities;

public enum OrderStatus { Pending, Confirmed, Paid, Cancelled }

/// <summary>
/// An order goes through a strict lifecycle:
/// Pending → Confirmed → Paid, and can be Cancelled from Pending/Confirmed.
/// Illegal transitions throw instead of silently printing an error.
/// </summary>
public sealed class Order
{
    private readonly List<OrderItem> _items = new();

    public int Id { get; }
    public Customer Customer { get; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public IPaymentMethod? PaymentMethod { get; set; }

    public IReadOnlyList<OrderItem> Items => _items;
    public decimal Total => _items.Sum(i => i.LineTotal);

    public Order(int id, Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        Id = id;
        Customer = customer;
    }

    public void AddItem(OrderItem item)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Items can only be added while the order is Pending.");

        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
    }

    public void RemoveItem(OrderItem item)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Items can only be removed while the order is Pending.");

        _items.Remove(item);
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"Cannot confirm an order in status {Status}.");
        if (_items.Count == 0)
            throw new InvalidOperationException("Cannot confirm an empty order.");

        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Paid or OrderStatus.Cancelled)
            throw new InvalidOperationException($"Cannot cancel an order in status {Status}.");

        Status = OrderStatus.Cancelled;
    }

    public void Pay()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("The order must be confirmed before payment.");

        var method = PaymentMethod
            ?? throw new InvalidOperationException("Choose a payment method first.");

        // reduce stock only when the payment succeeds
        foreach (var item in _items)
            item.Product.DecreaseStock(item.Quantity);

        method.Process(Total);
        Status = OrderStatus.Paid;
    }

    public override string ToString()
        => $"Order #{Id} for {Customer.Name} — {Status} — total {Total:0.00}";
}
