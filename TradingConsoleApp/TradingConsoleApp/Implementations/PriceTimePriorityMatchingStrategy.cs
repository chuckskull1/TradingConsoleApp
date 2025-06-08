namespace TradingSystem;

public class PriceTimePriorityMatchingStrategy : IMatchingStrategy
{
    public IEnumerable<Trade> MatchOrders(string symbol, IOrderBook orderBook)
    {
        var trades = new List<Trade>();
        var buys = orderBook.GetBuyOrders(symbol).ToList();
        var sells = orderBook.GetSellOrders(symbol).ToList();
        int b = 0, s = 0;
        while (b < buys.Count && s < sells.Count)
        {
            var buy = buys[b];
            var sell = sells[s];
            if (buy.Price >= sell.Price)
            {
                var tradeQty = Math.Min(buy.Quantity, sell.Quantity);
                var tradePrice = sell.Price; // price is usually the sell price in price‑time priority

                trades.Add(new Trade(Guid.NewGuid(), buy.Id, sell.Id, symbol, tradeQty, tradePrice, DateTime.UtcNow));

                // update in book (could be handled elsewhere)
                buy = buy with { Quantity = buy.Quantity - tradeQty };
                sell = sell with { Quantity = sell.Quantity - tradeQty };

                if (buy.Quantity == 0) b++; else buys[b] = buy;
                if (sell.Quantity == 0) s++; else sells[s] = sell;
            }
            else break; // best buy cannot meet best sell => done
        }
        return trades;
    }
}