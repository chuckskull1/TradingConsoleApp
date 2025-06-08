namespace TradingSystem;

public static class OrderFactory
{
    public static Order Create(Guid userId, OrderType type, string symbol, int qty, decimal price)
    {
        return new Order(
            Guid.NewGuid(), userId, type, symbol, qty, price,
            DateTime.UtcNow, OrderStatus.Accepted);
    }
}