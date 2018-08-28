using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using No7.Solution.Interfaces;

namespace No7.Solution.RecordLocationEntities
{
    public class DbTradeRecordDestination:ITradeRecordDestination
    {
        private readonly string _connectionString;

        public DbTradeRecordDestination(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentException($"{nameof(connectionString)} must be not null value");
        }
        public void WriteRecords(IEnumerable<TradeRecord> records)
        {

            int recordCounter = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var trade in records)
                    {
                        var command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "dbo.Insert_Trade";
                        command.Parameters.AddWithValue("@sourceCurrency", trade.SourceCurrency);
                        command.Parameters.AddWithValue("@destinationCurrency", trade.DestinationCurrency);
                        command.Parameters.AddWithValue("@lots", trade.Lots);
                        command.Parameters.AddWithValue("@price", trade.Price);

                        command.ExecuteNonQuery();
                        recordCounter++;
                    }

                    transaction.Commit();
                }

                connection.Close();
            }

            LoggerService.Info($"{recordCounter} trades in processed");
        }
    }
}
