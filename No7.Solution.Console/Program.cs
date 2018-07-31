namespace No7.Solution.Console
{
    using System.Reflection;
    class Program
    {
        static void Main(string[] args)
        {
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("No7.Solution.Console.trades.txt");
            var tradeProcessor = new NewTradeHandler(tradeStream);

            tradeProcessor.HandleTrades();

            System.Console.ReadKey();
        }
    }
}