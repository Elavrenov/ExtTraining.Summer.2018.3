using System.Configuration;
using No7.Solution.Interfaces;
using No7.Solution.RecordLocationEntities;
using No7.Solution.ServiceEntities;

namespace No7.Solution.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceFile = ConfigurationManager.AppSettings["TradeFile"];
            string connectionString = ConfigurationManager.ConnectionStrings["TradeData"].ConnectionString;

            LoggerService.Logger = ConsoleLogger.Instance;

            ITradeRecordSource source = new FileTradeRecordSource(sourceFile);
            ITradeRecordDestination destination = new DbTradeRecordDestination(connectionString);

            IInfoTransferService service = TradeRecordTransferService.Instance;

            service.TransferInfo(source, destination, TradeRecordValidator.Instance, new TradeRecordCreator());

            System.Console.ReadKey();
        }
    }
}