namespace SalesManagementSystem.Domain.Payments;

/// <summary>Strategy interface — each payment method knows how to charge.</summary>
public interface IPaymentMethod
{
    string Name { get; }
    void Process(decimal amount);
}

public sealed class CashPayment : IPaymentMethod
{
    public string Name => "Cash";
    public void Process(decimal amount)
        => Console.WriteLine($"  Paid {amount:0.00} in cash.");
}

public sealed class CreditCardPayment : IPaymentMethod
{
    private readonly string _cardLast4;

    public CreditCardPayment(string cardLast4 = "****")
    {
        _cardLast4 = cardLast4;
    }

    public string Name => "Credit card";
    public void Process(decimal amount)
        => Console.WriteLine($"  Charged {amount:0.00} to card ending {_cardLast4}.");
}

public sealed class BankTransferPayment : IPaymentMethod
{
    private readonly string _iban;

    public BankTransferPayment(string iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
            throw new ArgumentException("IBAN is required.", nameof(iban));
        _iban = iban;
    }

    public string Name => "Bank transfer";
    public void Process(decimal amount)
        => Console.WriteLine($"  Transferred {amount:0.00} to account {_ibannPreview()}.");
        
    private string _ibannPreview() => _iban.Length <= 6 ? _iban : "***" + _iban[^4..];
}
