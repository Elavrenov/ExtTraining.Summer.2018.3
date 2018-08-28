using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using No7.Solution.Interfaces;

namespace No7.Solution
{
    public class TradeRecordValidator : ITradeRecordValidator
    {
        private const int StandartFieldLength = 3;

        private static readonly Lazy<TradeRecordValidator> LazyTradeRecordValidator =
            new Lazy<TradeRecordValidator>(() => new TradeRecordValidator());

        public static TradeRecordValidator Instance => LazyTradeRecordValidator.Value;

        private TradeRecordValidator()
        {
        }

        public void CheckInfo(string destinationCurrency, string sourceCurrency, string lots, string price)
        {
            if (destinationCurrency == null)
            {
                throw new ArgumentNullException($"{nameof(destinationCurrency)} can't be null");
            }

            if (sourceCurrency == null)
            {
                throw new ArgumentNullException($"{nameof(sourceCurrency)} can't be null");
            }

            if (price == null)
            {
                throw new ArgumentNullException($"{nameof(price)} can't be null");
            }

            if (lots == null)
            {
                throw new ArgumentNullException($"{nameof(lots)} can't be null");
            }

            if (destinationCurrency.Length != StandartFieldLength )
            {
                throw new ArgumentException($"{nameof(destinationCurrency)} is invalid");
            }

            if (sourceCurrency.Length != StandartFieldLength )
            {
                throw new ArgumentException($"{nameof(sourceCurrency)} is invalid");
            }

            // Изменение культурных настроек для корректной записи в бд
            if (!float.TryParse(lots, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out _))
            {
                throw new ArgumentException($"{nameof(destinationCurrency)} is invalid");
            }

            // Изменение культурных настроек для корректной записи в бд
            if (!decimal.TryParse(price, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out _))
            {
                throw new ArgumentException($"{nameof(destinationCurrency)} is invalid");
            }
        }
    }
}
