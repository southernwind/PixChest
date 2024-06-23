using System;

using Microsoft.Extensions.Logging;

namespace PixChest.Database {
    /// <summary>
    /// ログ出力クラス
    /// </summary>
    internal class PixChestDbLoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// ロガーの作成
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new ConsoleLogger();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// コンソール出力ロガー
        /// </summary>
        private class ConsoleLogger : ILogger
        {
            IDisposable? ILogger.BeginScope<TState>(TState state)
            {
                return null;
            }

            bool ILogger.IsEnabled(LogLevel logLevel)
            {
                return logLevel > LogLevel.Debug;
            }

            void ILogger.Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception? exception,
                Func<TState, Exception?, string> formatter)
            {
                Console.WriteLine(formatter(state, exception));
            }
        }
    }
}
