namespace TradingSystem;

public class OrderService : IOrderService
{
    private readonly IUserRepository _users;
    private readonly IOrderRepository _orders;
    private readonly IOrderBook _orderBook;

    public OrderService(IUserRepository users, IOrderRepository orders, IOrderBook book)
    {
        _users = users; _orders = orders; _orderBook = book;
    }

    public Order PlaceOrder(Guid userId, OrderType type, string symbol, int qty, decimal price)
    {
        if (!_users.TryGet(userId, out _)) throw new ArgumentException("Unknown user");
        var order = OrderFactory.Create(userId, type, symbol, qty, price);
        _orders.Add(order);
        _orderBook.Add(order);
        return order;
    }

    public bool CancelOrder(Guid orderId)
    {
        if (_orders.TryGet(orderId, out var order))
        {
            var cancelled = order with { Status = OrderStatus.Cancelled };
            _orders.Update(cancelled);
            _orderBook.Remove(orderId);
            return true;
        }
        return false;
    }

    public bool ModifyOrder(Guid orderId, int newQty, decimal newPrice)
    {
        if (_orders.TryGet(orderId, out var order) && order.Status == OrderStatus.Accepted)
        {
            var modified = order with { Quantity = newQty, Price = newPrice, Timestamp = DateTime.UtcNow };
            _orders.Update(modified);
            _orderBook.Remove(orderId);
            _orderBook.Add(modified);
            return true;
        }
        return false;
    }

    public Order? GetOrder(Guid orderId) => _orders.TryGet(orderId, out var o) ? o : null;
}
