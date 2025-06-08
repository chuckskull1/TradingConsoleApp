namespace TradingSystem;

public interface ITradePublisher
{
    event Action<Trade>? TradeExecuted;
    void Publish(Trade trade);
}