namespace TradingSystem;

public interface IOrderService
{
    Order PlaceOrder(Guid userId, OrderType type, string symbol, int qty, decimal price);
    bool CancelOrder(Guid orderId);
    bool ModifyOrder(Guid orderId, int newQty, decimal newPrice);
    Order? GetOrder(Guid orderId);
}