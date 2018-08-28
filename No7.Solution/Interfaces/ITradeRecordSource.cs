using System.Collections.Generic;

namespace No7.Solution.Interfaces
{
    public interface ITradeRecordSource
    {
        IEnumerable<TradeRecord> ReadRecords(ITradeRecordValidator validator, IRecordFactory recordFactory);
    }
}
