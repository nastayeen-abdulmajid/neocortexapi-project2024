using MyCloudProject.Common;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading;
using MyExperiment;
using System.Threading.Tasks;
using Azure.Storage.Queues.Models;
using Azure.Storage.Queues;
using System.Text.Json;
using System.Text;

namespace MyCloudProject
{
    class Program
    {
        /// <summary>
        /// Your project ID from the last semester.
        /// </summary>
        private static string _projectName = "ML 22/23-7 Implement UnitTests for AdaptSegments";

        static async Task Main(string[] args)
        {
            CancellationTokenSource tokeSrc = new CancellationTokenSource();

            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                tokeSrc.Cancel();
            };

            Console.WriteLine($"Started experiment: {_projectName}");

            // Init configuration
            var cfgRoot = Common.InitHelpers.InitConfiguration(args);
            var cfgSec = cfgRoot.GetSection("MyConfig");

            // InitLogging
            var logFactory = InitHelpers.InitLogging(cfgRoot);
            var logger = logFactory.CreateLogger("Train.Console");

            logger?.LogInformation($"{DateTime.Now} - Started experiment: {_projectName}");

            IStorageProvider storageProvider = new AzureStorageProvider(cfgSec);
            IExperiment experiment = new Experiment(cfgSec, storageProvider, logger);

            //
            // Implements the step 3 in the architecture picture.
            while (!tokeSrc.Token.IsCancellationRequested)
            {
                // Step 3
                logger.LogInformation($"{DateTime.Now} - Waiting for experiment request...");
                IExerimentRequest request = await storageProvider.ReceiveExperimentRequestAsync(tokeSrc.Token);

                if (request != null)
                {
                    try
                    {
                        logger.LogInformation($"{DateTime.Now} - Received experiment request: {JsonSerializer.Serialize(request)}");
                        // Placeholder for processing the experiment request
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"{DateTime.Now} - Error occurred during experiment processing: {ex.Message}");
                    }
                }
                else
                {
                    await Task.Delay(500);
                    logger.LogTrace($"{DateTime.Now} - Queue empty...");
                }
            }
        }
    }
}
