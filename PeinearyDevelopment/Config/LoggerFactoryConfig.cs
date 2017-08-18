using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace PeinearyDevelopment.Config
{
    public static class LoggerFactoryConfig
    {
        public static void Configure(this ILoggerFactory loggerFactory, IConfigurationRoot configuration)
        {
            loggerFactory.AddConsole(configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            Log.Logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(configuration)
                                .WriteTo.RollingFile("logs/log-{Date}.txt")
                                .CreateLogger();
            loggerFactory.AddSerilog();
        }
    }
}
