namespace TradingSystem;

public interface IOrderRepository : IRepository<Order>
{
    void Update(Order order);
}