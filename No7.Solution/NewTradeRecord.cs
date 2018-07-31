﻿namespace No7.Solution
{
    public class NewTradeRecord
    {
        // Исспользование автосвойств предпочительнее для описания строк в таблице бд
        // Для описания новой строки таблицы бд потребуется лишь добавить автосвойство, описывающее его
        // Присутвует возможность получить любую строку, при этом она закрыта для измения клиентом
        // Инициализация возможна только через коструктор класса
        public string DestinationCurrency { get; }
        public float Lots { get; }
        public decimal Price { get; }
        public string SourceCurrency { get; }

        // Присутвует возможность расширешия и изменения бд
        // Инициализация объекта класса не через конструктор иррациональна
        // При изменении полей таблицы в конструктор можно просто добавить дополнительные поля
        public NewTradeRecord(string destinationCurrency, float lots, decimal price, string sourceCurrency)
        {
            DestinationCurrency = destinationCurrency;
            Lots = lots;
            Price = price;
            SourceCurrency = sourceCurrency;
        }

    }
}
