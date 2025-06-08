namespace TradingSystem;

public class TradePublisher : ITradePublisher
{
    public event Action<Trade>? TradeExecuted;
    public void Publish(Trade trade) => TradeExecuted?.Invoke(trade);
}