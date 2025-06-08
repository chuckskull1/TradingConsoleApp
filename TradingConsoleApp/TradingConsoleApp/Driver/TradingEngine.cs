namespace TradingSystem;

public class TradingEngine
{
    private readonly IOrderBook _orderBook;
    private readonly ITradeRepository _trades;
    private readonly IMatchingStrategy _matcher;
    private readonly ITradePublisher _publisher;
    private readonly ReaderWriterLockSlim _lock = new();

    public TradingEngine(IOrderBook orderBook, ITradeRepository trades, IMatchingStrategy matcher, ITradePublisher publisher)
    {
        _orderBook = orderBook; _trades = trades; _matcher = matcher; _publisher = publisher;
    }

    public void RunMatching(string symbol)
    {
        _lock.EnterWriteLock();
        try
        {
            var matches = _matcher.MatchOrders(symbol, _orderBook);
            foreach (var trade in matches)
            {
                _trades.Add(trade);
                _publisher.Publish(trade);
            }
        }
        finally { _lock.ExitWriteLock(); }
    }
}