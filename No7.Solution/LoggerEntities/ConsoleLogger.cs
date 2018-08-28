using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace No7.Solution
{
    public class ConsoleLogger : ILogger
    {
        private static readonly Lazy<ConsoleLogger> LazyLogger =
            new Lazy<ConsoleLogger>(() => new ConsoleLogger());

        public static ConsoleLogger Instance => LazyLogger.Value;

        private ConsoleLogger()
        {
            
        }
        public void Info(string msg)
        {
            ToConsole(msg, "info");
        }

        public void Warning(string msg)
        {
            ToConsole(msg, "warning");
        }

        public void Error(string msg)
        {
            ToConsole(msg, "error");
        }

        #region private members

        private void ToConsole(string msg, string type)
        {
            var logText = ($"{DateTime.Now:dd.MM.yyyy HH:mm:ss.fff} {type.ToUpper()}: {msg}");
            Console.WriteLine(logText);
        }

        #endregion
    }
}
