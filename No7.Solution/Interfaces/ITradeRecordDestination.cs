using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace No7.Solution.Interfaces
{
    public interface ITradeRecordDestination
    {
        void WriteRecords(IEnumerable<TradeRecord> records);
    }
}
