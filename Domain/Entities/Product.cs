namespace SalesManagementSystem.Domain.Entities;

/// <summary>A sellable product with price and stock.</summary>
public sealed class Product
{
    private decimal _price;
    private int _stockQuantity;

    public int Id { get; }
    public string Name { get; }

    public decimal Price
    {
        get => _price;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Price cannot be negative.");
            _price = value;
        }
    }

    public int StockQuantity
    {
        get => _stockQuantity;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Stock cannot be negative.");
            _stockQuantity = value;
        }
    }

    public Product(int id, string name, decimal price, int stockQuantity)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Product name required.", nameof(name));

        Id = id;
        Name = name.Trim();
        Price = price;
        StockQuantity = stockQuantity;
    }

    public bool HasStock(int quantity) => quantity > 0 && _stockQuantity >= quantity;

    public void IncreaseStock(int amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        StockQuantity += amount;
    }

    public void DecreaseStock(int amount)
    {
        if (!HasStock(amount))
            throw new InvalidOperationException($"Not enough stock for '{Name}'.");
        StockQuantity -= amount;
    }

    public override string ToString() => $"#{Id} {Name,-20} {Price,10:0.00} (stock {StockQuantity})";
}
