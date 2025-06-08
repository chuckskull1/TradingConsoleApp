namespace TradingSystem;

public interface IMatchingStrategy
{
    IEnumerable<Trade> MatchOrders(string symbol, IOrderBook orderBook);
}