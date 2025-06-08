namespace TradingSystem;

public interface IOrderBook
{
    void Add(Order order);
    void Remove(Guid orderId);
    bool TryGet(Guid orderId, out Order order);
    IEnumerable<Order> GetBuyOrders(string symbol);
    IEnumerable<Order> GetSellOrders(string symbol);
}