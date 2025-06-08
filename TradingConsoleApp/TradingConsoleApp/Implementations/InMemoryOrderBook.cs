using System.Collections.Concurrent;
namespace TradingSystem;

public class InMemoryOrderBook : IOrderBook
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, Order>> _ordersBySymbol = new();

    public void Add(Order order)
    {
        var book = _ordersBySymbol.GetOrAdd(order.Symbol, _ => new());
        book[order.Id] = order;
    }

    public void Remove(Guid orderId)
    {
        foreach (var symbolBook in _ordersBySymbol.Values)
            symbolBook.TryRemove(orderId, out _);
    }

    public bool TryGet(Guid orderId, out Order order)
    {
        foreach (var symbolBook in _ordersBySymbol.Values)
            if (symbolBook.TryGetValue(orderId, out order)) return true;
        order = null!;
        return false;
    }

    public IEnumerable<Order> GetBuyOrders(string symbol) =>
        _ordersBySymbol.GetOrAdd(symbol, _ => new()).Values
            .Where(o => o.Type == OrderType.Buy && o.Status == OrderStatus.Accepted)
            .OrderByDescending(o => o.Price).ThenBy(o => o.Timestamp);

    public IEnumerable<Order> GetSellOrders(string symbol) =>
        _ordersBySymbol.GetOrAdd(symbol, _ => new()).Values
            .Where(o => o.Type == OrderType.Sell && o.Status == OrderStatus.Accepted)
            .OrderBy(o => o.Price).ThenBy(o => o.Timestamp);
}