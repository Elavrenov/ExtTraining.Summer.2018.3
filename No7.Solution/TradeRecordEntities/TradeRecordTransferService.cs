using System;
using No7.Solution.Interfaces;

namespace No7.Solution.ServiceEntities
{
    public class TradeRecordTransferService : IInfoTransferService
    {
        private static readonly Lazy<TradeRecordTransferService> LazyService =
            new Lazy<TradeRecordTransferService>(() => new TradeRecordTransferService());

        public static TradeRecordTransferService Instance => LazyService.Value;

        public float LotSize { get; set; } = 100000f;

        private TradeRecordTransferService()
        {
        }

        public void TransferInfo(ITradeRecordSource source,
            ITradeRecordDestination destination,
            ITradeRecordValidator validator,
            IRecordFactory recordFactory)
        {
            var records = source.ReadRecords(validator, recordFactory);
            destination.WriteRecords(records);
        }
    }
}
