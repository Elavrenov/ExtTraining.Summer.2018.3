using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace No7.Solution.Interfaces
{
    public interface IRecordFactory
    {
        TradeRecord CreateNewRecord(string destinationCurrency, string sourceCurrency, string price, string lots, ITradeRecordValidator validator);
    }
}
