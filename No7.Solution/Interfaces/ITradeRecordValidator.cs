using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace No7.Solution.Interfaces
{
    public interface ITradeRecordValidator
    {
        void CheckInfo(string destinationCurrency, string sourceCurrency, string lots, string price);
    }
}
