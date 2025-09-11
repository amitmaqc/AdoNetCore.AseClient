using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Configuration;

namespace AdoNetCore.AseClient.Internal
{
    public sealed class Logger
    {
#if DEBUG
        private static Logger _instance;
        private static ILogger<AseConnection> _logger;
        
        static Logger()
        {
            Initialize();
        }

#endif
        public static Logger Instance
        {
            get
            {
#if RELEASE
                return null;
#else
                return _instance;
#endif
            }
        }

        public static void Initialize()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
            {
                builder
                    .AddConfiguration(configuration.GetSection("Logging"))
                    .AddConsole();
            });
            _logger = LoggerFactory.CreateLogger<AseConnection>();
            Enable();
        }

        public static void Enable(bool toConsole = true, bool toDebug = false, bool timestamps = false)
        {
#if DEBUG
            _instance = new Logger
            {
                ToConsole = toConsole,
                ToDebug = toDebug,
                Timestamps = timestamps
            };
#endif
        }

        public static void Disable()
        {
#if DEBUG
            _instance = null;
#endif
        }

        private bool ToConsole { get; set; } = false;
        private bool ToDebug { get; set; }
        private bool Timestamps { get; set; } = true;

        private bool _lineStart = true;

        private Logger() { }

        private string Timestamp => _lineStart && Timestamps ? DateTime.UtcNow.ToString("[yyyy-MM-dd HH:mm:ss] ") : string.Empty;

        public void WriteLine()
        {
            if (ToConsole) Console.WriteLine();
            if (ToDebug) Debug.WriteLine(string.Empty);
        }

        public void WriteLine(string line, LogLevel level = LogLevel.Information)
        {
            Log(level, line);
            var formatted = $"{Timestamp}{line}";
            if (ToConsole) Console.WriteLine(formatted);
            if (ToDebug) Debug.WriteLine(formatted);
            _lineStart = true;
        }

        public void Write(string value, LogLevel level = LogLevel.Information)
        {
            Log(level, value);
            var formatted = $"{Timestamp}{value}";
            if (ToConsole) Console.Write(formatted);
            if (ToDebug) Debug.Write(formatted);
            _lineStart = false;
        }

        public void Log(LogLevel level, string message)
        {
            _logger.Log(level, message);
        }
    }
}
