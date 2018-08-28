using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace No7.Solution
{
    public static class LoggerService
    {
        public static ILogger Logger { get; set; }

        #region private members
        public static void Warning(string msg)
        {
            ThrowInvalidData(msg);
            Logger.Warning(msg);
        }

        public static void Info(string msg)
        {
            ThrowInvalidData(msg);
            Logger.Info(msg);
        }

        public static void Error(string msg)
        {
            ThrowInvalidData(msg);
            Logger.Error(msg);
        }
        private static void ThrowInvalidData(string msg)
        {
            if (Logger == null)
            {
                throw new ArgumentException($"{nameof(Logger)} can't be null");
            }

            if (msg == null)
            {
                throw new ArgumentException($"{nameof(msg)} can't be null");
            }
        }

        #endregion

    }
}
