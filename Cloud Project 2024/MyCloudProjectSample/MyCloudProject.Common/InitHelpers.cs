using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Logging.Console;

namespace MyCloudProject.Common
{
    public static class InitHelpers
    {
        /// <summary>
        /// Create Logging infrastructure in the Trainer Workload.
        /// Todo: Make sure that the logging level can be configured on start of the application.
        /// </summary>
        /// <returns></returns>
        public static ILoggerFactory InitLogging(IConfigurationRoot configRoot)
        {
            //create logger from the appsettings addConsole Debug to Logg 
            return LoggerFactory.Create(logBuilder =>
            {
                ConsoleLoggerOptions logCfg = new ConsoleLoggerOptions();

                logBuilder.AddConfiguration(configRoot.GetSection("Logging"));

                logBuilder.AddConsole((opts) =>
                {
                    opts.IncludeScopes = true;
                }).AddDebug();
            });
        }

        // Placeholder for configuration initialization
    }
}
