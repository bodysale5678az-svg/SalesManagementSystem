namespace SalesManagementSystem.Domain.Entities;

/// <summary>One line of an order: a product and how many units were bought.</summary>
public sealed class OrderItem
{
    public Product Product { get; }
    public int Quantity { get; }

    public OrderItem(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive.");

        Product = product;
        Quantity = quantity;
    }

    /// <summary>Price is captured at order time via the product, so totals stay in sync.</summary>
    public decimal LineTotal => Product.Price * Quantity;

    public override string ToString() => $"{Quantity} x {Product.Name,-18} = {LineTotal,10:0.00}";
}
