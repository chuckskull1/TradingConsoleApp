namespace TradingSystem;

public record Order(
    Guid Id,
    Guid UserId,
    OrderType Type,
    string Symbol,
    int Quantity,
    decimal Price,
    DateTime Timestamp,
    OrderStatus Status);