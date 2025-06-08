namespace TradingSystem;

public record Trade(
    Guid Id,
    Guid BuyerOrderId,
    Guid SellerOrderId,
    string Symbol,
    int Quantity,
    decimal Price,
    DateTime Timestamp);