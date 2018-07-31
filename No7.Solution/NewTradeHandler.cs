﻿namespace No7.Solution
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    public class NewTradeHandler
    {
        #region Const

        // Исспользование статической field-like инициализированной переменной неприемлемо
        // Необходимо исспользовать именованную константу
        private const float LotSize = 100000f;

        // Исспользование именованных констант позволит вносить измения в логику валидации без существенного рефакторинга
        private const int StandartHeaderLength = 0;
        private const int StandartFieldLength = 3;
        private const int StandartCurrencyLength = 6;

        #endregion

        #region Fields

        // Работа с этими полями осуществляется во всем классе
        // Если эти поля являются локальными переменными (как было раньше)
        // Отсутствует возможность расширения кода в принципе
        private readonly List<NewTradeRecord> _trades;
        private readonly List<string> _lines;

        #endregion

        #region Ctors

        // Создание и инициализация объектов, для дальнейшей работы с ними, это не обязанность метода HandleTrades
        // А обязанность констуктора 
        // Тут же выполняется и работа с потоком Stream
        public NewTradeHandler(Stream stream)
        {
            _trades = new List<NewTradeRecord>();
            _lines = new List<string>();

            using (var reader = new StreamReader(stream))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    _lines.Add(line);
                }
            }
        }

        #endregion

        #region Public API

        // Теперь передача параметра Stream не нужна вообще
        // Метод будет занят только своим делом
        public void HandleTrades()
        {
            var lineCount = 1;

            // Делегирование логики валидации и добавления нового поля другому методу
            foreach (var line in _lines)
            {
                AddField(line, ref lineCount);
            }

            // Вызов метода сохранения в бд
            SaveIntoDb(ConfigurationManager.ConnectionStrings["TradeData"].ConnectionString);

            System.Console.WriteLine("INFO: {0} trades processed", _trades.Count);
        }

        #endregion

        #region Private methods

        // Новый метод для работы и добавления поля
        // Необходим для разделения логики и последующей возможности расширяемости кода
        // (или измения логики добавления нового поля)
        private void AddField(string line, ref int lineCount)
        {
            var fields = line.Split(new char[] { ',' });

            // Определение логики валидации в новом методе
            // Используется ValueTuples для возврата нескольких параметров, если валидация успешна
            // нули в обратном случае
            var info = ValidateParameters(fields, lineCount);

            // Использование именованных констант
            var sourceCurrencyCode = fields[StandartHeaderLength].Substring(StandartHeaderLength, StandartFieldLength);
            var destinationCurrencyCode = fields[StandartHeaderLength].Substring(StandartFieldLength, StandartFieldLength);

            // Исспользование конструктора для создания объекта класса
            _trades.Add(new NewTradeRecord(destinationCurrencyCode, info.Item1 / LotSize, info.Item2, sourceCurrencyCode));

            lineCount++;
        }

        // Логика валидации должна выделяться отдельно
        // Благодоря этому присутвует возможность добавить\убрать условия валидации
        private (int, decimal) ValidateParameters(string[] fields, int lineCount)
        {
            // Появляется дополнительная возможность контролировать строки
            // Валидация проводится в нужном порядке
            var counter = 0;
            var tradeAmount = 0;
            var tradePrice = 0M;

            // Оптимизация за счет именованных констант
            while (counter < StandartFieldLength)
            {
                if (fields.Length != StandartFieldLength)
                {
                    System.Console.WriteLine("WARN: Line {0} malformed. Only {1} field(s) found.", lineCount, fields.Length);
                }

                if (fields[counter].Length != StandartCurrencyLength)
                {
                    System.Console.WriteLine("WARN: Trade currencies on line {0} malformed: '{1}'", lineCount, fields[counter]);
                }

                // Изменение культурных настроек для корректной записи в бд
                if (!int.TryParse(fields[++counter], NumberStyles.Integer, CultureInfo.InvariantCulture, out tradeAmount))
                {
                    System.Console.WriteLine("WARN: Trade amount on line {0} not a valid integer: '{1}'", lineCount, fields[counter]);
                }

                // Изменение культурных настроек для корректной записи в бд
                if (!decimal.TryParse(fields[++counter], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out tradePrice))
                {
                    System.Console.WriteLine("WARN: Trade price on line {0} not a valid decimal: '{1}'", lineCount, fields[counter]);
                }

                counter++;
            }

            return (tradeAmount, tradePrice);
        }

        // Сохранение в бд данных это не обязанность метода HandleTrades
        // Всегда присутсвует возможность измениния бд (возможность записи в несколько баз данных)
        // Рациональнее будет выделить отдельный приватный метод
        private void SaveIntoDb(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var trade in _trades)
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
                    }

                    transaction.Commit();
                }

                connection.Close();
            }
        }

        #endregion
    }
}