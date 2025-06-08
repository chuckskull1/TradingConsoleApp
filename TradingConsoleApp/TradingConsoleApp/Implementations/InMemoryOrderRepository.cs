namespace TradingSystem;

public class InMemoryOrderRepository : InMemoryRepository<Order>, IOrderRepository
{
    public void Update(Order order) => store[order.Id] = order;
}