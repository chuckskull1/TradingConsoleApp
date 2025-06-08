using System;

namespace TradingSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var userRepo = new InMemoryUserRepository();
            var orderRepo = new InMemoryOrderRepository();
            var tradeRepo = new InMemoryTradeRepository();
            var orderBook = new InMemoryOrderBook();
            var publisher = new TradePublisher();
            var matcher = new PriceTimePriorityMatchingStrategy();
            var engine = new TradingEngine(orderBook, tradeRepo, matcher, publisher);
            var orderService = new OrderService(userRepo, orderRepo, orderBook);

            // subscribe to trade notifications
            publisher.TradeExecuted += t => Console.WriteLine($"TRADE: {t.Quantity} {t.Symbol} @ {t.Price} between {t.BuyerOrderId} and {t.SellerOrderId}");

            var Rohit = new User(Guid.NewGuid(), "Rohit", "9999999999", "rohit@example.com");
            var Akshay   = new User(Guid.NewGuid(), "Akshay",   "8888888888", "akshay@example.com");
            var Rahul = new User(Guid.NewGuid(), "Rahul", "7777777777", "rahul@example.com");
            userRepo.Add(Rohit); userRepo.Add(Akshay); userRepo.Add(Rahul);

            // Basic Buy/Sell Match
            var buy1 = orderService.PlaceOrder(Rohit.Id, OrderType.Buy, "Google", 100, 510.5m);
            var sell1 = orderService.PlaceOrder(Akshay.Id, OrderType.Sell, "Google", 100, 509m);
            engine.RunMatching("Google");

            // Partial fill
            var buy2 = orderService.PlaceOrder(Rohit.Id, OrderType.Buy, "PhonePe", 200, 1450m);
            var sell2 = orderService.PlaceOrder(Rahul.Id, OrderType.Sell, "PhonePe", 100, 1449m);
            engine.RunMatching("PhonePe");

            // Cancel order
            var orderToCancel = orderService.PlaceOrder(Akshay.Id, OrderType.Sell, "Microsoft", 100, 3200m);
            orderService.CancelOrder(orderToCancel.Id);

            // Modify order
            var orderToModify = orderService.PlaceOrder(Rohit.Id, OrderType.Buy, "Microsoft", 50, 3195m);
            orderService.ModifyOrder(orderToModify.Id, 100, 3201m);
            orderService.PlaceOrder(Akshay.Id, OrderType.Sell, "Microsoft", 100, 3190m);
            engine.RunMatching("Microsoft");

            Console.WriteLine("Displaying all the orders");
            foreach (var o in orderRepo.All())
                Console.WriteLine(o);

            Console.WriteLine("Displaying all the Trades");
            foreach (var t in tradeRepo.All())
                Console.WriteLine(t);

        }
    }
}