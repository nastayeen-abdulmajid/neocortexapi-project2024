using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCloudProject.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyExperiment
{
    public class AzureStorageProvider : IStorageProvider
    {
        private MyConfig _config;
        private ILogger logger;

        public AzureStorageProvider(IConfigurationSection configSection)
        {
            _config = new MyConfig();
            configSection.Bind(_config);
        }
        /// <summary>
        /// Deletes a message from the Azure Storage Queue.
        /// </summary>
        /// <param name="request">The experiment request containing the MessageId and PopReceipt of the message to be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when a null argument is provided.</exception>
        /// <exception cref="ArgumentException">Thrown when an invalid argument is provided.</exception>
        /// <exception cref="RequestFailedException">Thrown when the Azure SDK request fails.</exception>
        /// <exception cref="Exception">Thrown for any other unexpected errors.</exception>
        public async Task CommitRequestAsync(IExerimentRequest request)
        {
            try
            {
                // Initialize the queue client using the storage connection string and queue name
                QueueClient queueClient = new QueueClient(this._config.StorageConnectionString, this._config.Queue);

                // Delete the message from the queue using the MessageId and PopReceipt
                await queueClient.DeleteMessageAsync(request.MessageId, request.PopReceipt);

                Console.WriteLine($"Message with ID {request.MessageId} deleted successfully.");
            }
            catch (ArgumentNullException ex)
            {
                // Handle specific exception when an argument is null
                Console.Error.WriteLine($"Argument null exception: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                // Handle specific exception when an argument is invalid
                Console.Error.WriteLine($"Argument exception: {ex.Message}");
            }
            catch (RequestFailedException ex)
            {
                // Handle Azure SDK request failed exception
                Console.Error.WriteLine($"Failed to delete message: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Downloads input files from Azure Blob Storage to a local directory.
        /// </summary>
        /// <param name="fileName">The name of the first file to download.</param>
        /// <param name="fileName1">The name of the second file to download.</param>
        /// <returns>The path of the directory where the files were downloaded.</returns>
        public async Task<string> DownloadInputAsync(string fileName, string fileName1)
        {
            // Create a BlobContainerClient to interact with the blob container
            BlobContainerClient container = new BlobContainerClient(_config.StorageConnectionString, _config.TrainingContainer);
            string downloadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "DownloadedFiles");

            // Delete the folder if it already exists and create a new one
            if (Directory.Exists(downloadFolderPath))
            {
                Directory.Delete(downloadFolderPath, true);
            }
            Directory.CreateDirectory(downloadFolderPath);

            // Download the first file
            BlobClient blob = container.GetBlobClient(fileName);
            string downloadFilePath = Path.Combine(downloadFolderPath, fileName);
            await blob.DownloadToAsync(downloadFilePath);

            // Download the second file
            BlobClient blob1 = container.GetBlobClient(fileName1);
            string downloadFilePath1 = Path.Combine(downloadFolderPath, fileName1);
            await blob1.DownloadToAsync(downloadFilePath1); // Use blob1 here

            return downloadFolderPath;
        }

        /// <summary>
        /// Receives and processes an experiment request message from the Azure Storage Queue.
        /// </summary>
        /// <param name="token">A cancellation token to monitor for cancellation requests.</param>
        /// <returns>The deserialized experiment request if available; otherwise, null.</returns>
        /// <exception cref="JsonException">Thrown when JSON deserialization fails.</exception>
        /// <exception cref="Exception">Thrown for any other unexpected errors.</exception>
        public async Task<IExerimentRequest> ReceiveExperimentRequestAsync(CancellationToken token)
        {
            QueueClient queueClient = new QueueClient(this._config.StorageConnectionString, this._config.Queue);

            while (!token.IsCancellationRequested)
            {
                QueueMessage[] messages = await queueClient.ReceiveMessagesAsync();

                if (messages != null && messages.Length > 0)
                {
                    foreach (var message in messages)
                    {
                        try
                        {
                            string msgTxt = message.Body.ToString();
                            await Console.Out.WriteLineAsync(msgTxt);

                            // Deserialize the message body to an experiment request
                            var request = JsonSerializer.Deserialize<ExerimentRequestMessage>(msgTxt);
                            if (request != null)
                            {
                                request.MessageId = message.MessageId;
                                request.PopReceipt = message.PopReceipt;
                                await Console.Out.WriteLineAsync($"Selected input file for DecrementPermanenceIfInactivePresynapticCells is : {request.DecrementPermanence_InputFile}");
                                await Console.Out.WriteLineAsync($"Selected input file for VerifyPermanenceBoundsAfterAdaptation is : {request.VerifyPermanence_InputFile}");
                                return request;
                            }
                        }
                        catch (JsonException ex)
                        {
                            // Handle JSON deserialization error
                            logger?.LogError($"{DateTime.Now} - JSON deserialization error: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            // Handle any other errors while processing the message
                            logger?.LogError($"{DateTime.Now} - An error occurred while processing the message: {ex.Message}");
                        }
                    }
                }

                await Task.Delay(1000); // Wait for 1 second before checking the queue again
            }

            return null;
        }

        /// <summary>
        /// Uploads the experiment result to Azure Blob Storage.
        /// </summary>
        /// <param name="result">The experiment result containing the output files to upload.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="NotImplementedException">Thrown if the method is not implemented.</exception>
        public Task UploadExperimentResult(IExperimentResult result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Uploads the result file to Azure Blob Storage.
        /// </summary>
        /// <param name="experimentName">The name of the experiment.</param>
        /// <param name="result">The experiment result containing the output file to upload.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UploadResultAsync(string experimentName, IExperimentResult result)
        {
            // Initialize the BlobContainerClient with the storage connection string and result container name
            BlobContainerClient container = new BlobContainerClient(_config.StorageConnectionString, _config.ResultContainer);
            await container.CreateIfNotExistsAsync(); // Create the container if it doesn't exist

            // List and delete all existing blobs in the container (optional, depending on requirements)
            await foreach (BlobItem blobItem in container.GetBlobsAsync())
            {
                BlobClient blobClient = container.GetBlobClient(blobItem.Name);
                await blobClient.DeleteIfExistsAsync();
            }
            string fileName = Path.GetFileName(result.OutputFiles);
            BlobClient blob = container.GetBlobClient(fileName);
            // Upload the file to Azure Blob Storage
            await blob.UploadAsync(result.OutputFiles, true);

            // Optionally, log successful upload
            logger?.LogInformation($"{DateTime.Now} - Uploaded file '{fileName}' to Azure Blob Storage");
        }

    }
}
