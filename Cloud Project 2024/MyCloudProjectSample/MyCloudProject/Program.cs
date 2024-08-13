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
        }
    }
}
