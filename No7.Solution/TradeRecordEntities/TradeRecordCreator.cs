using System.Globalization;
using No7.Solution.Interfaces;

namespace No7.Solution
{
    public class TradeRecordCreator : IRecordFactory
    {
        public TradeRecord CreateNewRecord(string destinationCurrency, string sourceCurrency, string price, string lots, ITradeRecordValidator validator)
        {
            validator.CheckInfo(destinationCurrency, sourceCurrency, price, lots);

            return new TradeRecord(
                destinationCurrency,
                sourceCurrency,
                float.Parse(lots, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.InvariantCulture),
                decimal.Parse(price, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.InvariantCulture)
                );
        }
    }
}
