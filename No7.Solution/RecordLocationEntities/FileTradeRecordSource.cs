using System;
using System.Collections.Generic;
using System.IO;
using No7.Solution.Interfaces;

namespace No7.Solution.RecordLocationEntities
{
    public class FileTradeRecordSource : ITradeRecordSource
    {
        private const int StandartFieldLength = 3;
        private const int StandartCurrencyLength = 6;
        private readonly string _sourcePath;

        public FileTradeRecordSource(string sourcePath)
        {
            if (sourcePath == null)
            {
                throw new ArgumentNullException(nameof(sourcePath));
            }

            if (!File.Exists(sourcePath))
            {
                throw new ArgumentException("File is not exist");
            }

            _sourcePath = sourcePath;
        }
        public IEnumerable<TradeRecord> ReadRecords(ITradeRecordValidator validator, IRecordFactory recordFactory)
        {
            if (validator == null)
            {
                throw new ArgumentNullException($"{nameof(validator)} can't be null");
            }

            if (recordFactory == null)
            {
                throw new ArgumentNullException($"{nameof(recordFactory)} can't be null");
            }

            var lineNumber = 0;

            using (var sr = new StreamReader(_sourcePath))
            {
                while (true)
                {
                    var recordLine = sr.ReadLine();

                    if (recordLine != null)
                    {
                        TradeRecord record = null;

                        try
                        {
                            record = CreateValidRecord(recordLine.Split(",".ToCharArray()), recordFactory, validator);
                        }
                        catch (ArgumentException e)
                        {
                            var logMessage = $"{e.Message}. Invalid record. Line:{lineNumber}";

                            LoggerService.Warning(logMessage);
                        }

                        if (record != null)
                        {
                            yield return record;
                        }

                        lineNumber++;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
        }

        private TradeRecord CreateValidRecord(string[] fields, IRecordFactory recordFactory, ITradeRecordValidator validator)
        {
            if (fields.Length != StandartFieldLength)
            {
                throw new ArgumentException("Not valid amount of fields");
            }

            if (fields[0].Length != StandartCurrencyLength)
            {
                throw new ArgumentException("Not valid currency codes");
            }

            var source = fields[0].Substring(0, 3);
            var destination = fields[0].Substring(3, 3);

            return recordFactory.CreateNewRecord(destination, source, fields[2], fields[1], validator);
        }
    }
}
