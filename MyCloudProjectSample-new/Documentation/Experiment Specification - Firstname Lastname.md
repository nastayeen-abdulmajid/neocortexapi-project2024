# Project : ML 22/23 - 7 Implement UnitTests  for AdaptSegments - Azure Cloud Implementation

## Table of contents

1. [Introduction](#-1.-introduction)
2. [Technologies Used](#-2.-Technologies-used)
3. [Our Objective](#-3.-Our-Objective)
4. [Project Architecture](#-4.-Project-Architecture)
5. [Prerequisites](#-5.-Prerequisites)
6. [Our input to the experiment](#-6.-Our-input-to-the-experiment)
7. [Run Experiment](#-7.-Run-Experiment)
8. [Our output to the experiment](#-8.-Our-output-of-the-experiment)
9. [About adapt segments method](#-9.-About-Adapt-segments-method)
10. [Azure implementation](#-10.-Azure-Implementation)
11. [How to run the experiment](#-11.-How-to-run-the-experiment)
12. [Describing our result](#-12.-Describing-our-Result)
13. [Consolidated information of our components](#-13.-Consolidated-Information-of-our-components)

## 1. Introduction
Our project is a cloud-based system designed to execute experiments by processing tasks from Azure Storage Queues. It integrates with Azure Storage to handle experiment requests, download necessary input files, run the experiment, upload the results, and manage the entire lifecycle of an experiment. The purpose of project was developed to automate the process of running experiments, particularly those related to machine learning. The goal is to make the experiment lifecycle efficient, scalable, and manageable in a cloud environment. 

HTM is a machine-learning framework inspired by the structure and function of the neocortex in the human brain. It focuses on time-based patterns, sequence learning, and anomaly detection. HTM models aim to replicate the brain's ability to learn and recognize temporal sequences continuously. In HTM, a "segment" is a group of synapses, which are connections between neurons. These segments can be thought of as small sub-patterns that neurons use to predict future activity. There are two primary types of segments in HTM:
* **Proximal Segments**: Used in the Spatial Pooler to form connections between the input and a neuron's dendrites, determining the neuron's activation.
* **Distal Segments**: Used in Temporal Memory to form connections between neurons and to predict future inputs based on previous patterns.

Adapt Segments refers to modifying the synapses on a segment based on the neuron's activity. This is how HTM systems learn from experience. The adaptation of segments occurs during both the learning and inference phases. In the learning phase, When a neuron is active, HTM adapts the segments by reinforcing or weakening the synapses on those segments. If a neuron correctly predicted its activation, the synapses that contributed to the correct prediction are strengthened (increasing permanence). Conversely, if the prediction was incorrect, those synapses might be weakened or even removed (decreasing permanence). During inference, HTM uses the segments to predict future inputs. As new data is received, the HTM system checks if the predictions are correct and adapts the segments accordingly. This continuous adaptation allows the model to improve its predictions over time.

The adaptation of segments is crucial for HTM's ability to learn sequences and make predictions. This process mimics how biological neurons adjust their synaptic connections based on experience, which is fundamental to learning in the brain.
* **Decrement Permanence**: In cases where predictions are incorrect, the permanence of synapses is decremented, potentially leading to their removal if they become too weak.
* **Increment Permanence**: When predictions are correct, the permanence of the associated synapses is incremented, strengthening the connections and reinforcing the correct prediction pathway.

## 2. Technologies used
1. Github
2. Visual Studio 2022
3. C# Programming language
4. Docker Desktop
5. JSON input file
6. Microsoft Azure Cloud
7. Output in excel file

## 3. Our Objective
Our research is focused on evaluating the AdaptSynapses method's effectiveness.
- We created unit test cases specifically designed to verify the functionality of the AdaptSynapses method.
- We developed a cloud-based system for automated test management, ensuring regular initiation, tracking, and evaluation of AdaptSegment unit tests.
- We built methods that allow test cases to be executed either with input files or predefined settings, enhancing flexibility.
- Designed and implemented a scalable cloud infrastructure to perform thorough unit testing of the HTM algorithm's AdaptSegment method.
- Successfully deployed the project as a Docker image to a cloud environment via a container registry, enabling the entire project to run in a container instance.

## 4. Project Architecture
![Architecture](https://github.com/nastayeen-abdulmajid/neocortexapi-project2024/blob/master/MyCloudProjectSample-new/Documentation/CC-images/Cloud%20project%20architecture.png)

### Explanation of project architecture
**Step 1**: 
* **GitHub repository**: A collaborative platform for version control, facilitating project source code management.
* **Visual Studio 2022**: A comprehensive IDE designed for coding, debugging, and testing software.
* **Docker desktop**: A containerization tool that encapsulates the application within a Docker image for deployment on Azure.
  
**Step 2**:   
* **Docker Image**: A lightweight, portable package that includes all dependencies required to run the application.
* **Azure Container Registry**: A service for storing and managing Docker images within Azure.

**Step 3**:  
* **Azure Container Instances**:  A service for running containerized applications on Azure, eliminating the need for infrastructure management.

**Step 4**: 
* **Azure Storage Queue**: A message queue that triggers the application's processing workflow upon receiving a queue message.

**Step 5,6,7,8**: 
* **Input Blob Storage**: It refer to blobs used to store data that will be consumed or processed by an application or service. For example, files or datasets that need to be read or analyzed are stored here.
* **Output Blob Storage**: This is used to store the results of processing or operations performed by an application or service. For example, generated reports, processed images, or log files are stored as output blobs.

## 5. Prerequisites
1. Visual Studio 2022: We installed it on our machine with the necessary workloads (ASP.NET, Azure, etc.). 
2. Azure Student Subscription: Active Azure account. 
3. Docker Desktop: Installed and running on our machine.

### Step 1: Setting Up Docker in our Adapt segments project

- Opening our existing project in Visual Studio 2022.
- Right-click on your project in the Solution Explorer. Select Add > Docker Support.
- Choosing the target OS (Linux is commonly used).
- Visual Studio will generate a Dockerfile in our project, which describes how your application will be containerized.

### Step 2: Build and Run our Docker Container Locally
To building the Docker Image:

- Right-click the project in Solution Explorer. Choose Build or Rebuild to create the Docker image locally.
- Run the Docker Container Locally:
- Press F5 or click on the Run button to start the container.
This ensures everything is working fine locally before deploying to Azure.

### Step 3: Deploying to Azure

- In the Azure Portal, we created a new Azure Container Registry(ACR)
- We Logged in to Azure Container Registry.
- Push Docker Image to ACR.
- In the Azure Portal, create a new Web App with Docker support or use the Azure CLI
- Set the container settings to point to your image in ACR.

## 6. Our input to the experiment

~~~ExperimentRequestMessage.cs
  using MyCloudProject.Common;
  using System;
  using System.Collections.Generic;
  using System.Text;

  namespace MyExperiment
  {
      /// <summary>
      /// Represents a request message for an experiment.
      /// Implements the IExerimentRequest interface to define the structure of an experiment request.
      /// </summary>
      internal class ExerimentRequestMessage : IExerimentRequest
      {
          /// <summary>
          /// Gets or sets the receipt of the message from the queue.
          /// Used to manage the visibility and deletion of the message in the queue.
          /// </summary>
          public string PopReceipt { get; set; }

          /// <summary>
          /// Gets or sets the unique identifier for the experiment.
          /// Used to track and identify the specific experiment request.
          /// </summary>
          public string ExperimentId { get; set; }

          /// <summary>
          /// Gets or sets the file path for the decrement permanence input data.
          /// This file contains the data used to decrement the permanence in the experiment.
          /// </summary>
          public string DecrementPermanence_InputFile { get; set; }

          /// <summary>
          /// Gets or sets the name of the experiment request.
          /// Provides a descriptive name for the experiment.
          /// </summary>
          public string Name { get; set; }

          /// <summary>
          /// Gets or sets the description of the experiment request.
          /// Provides additional information about the purpose or details of the experiment.
          /// </summary>
          public string Description { get; set; }

          /// <summary>
          /// Gets or sets the unique identifier for the message in the queue.
          /// Used to track and manage the specific message within the queue.
          /// </summary>
          public string MessageId { get; set; }

          /// <summary>
          /// Gets or sets the receipt associated with the message in the queue.
          /// Used to manage the visibility and deletion of the message in the queue.
          /// </summary>
          public string MessageReceipt { get; set; }

          /// <summary>
          /// Gets or sets the file path for the verify permanence input data.
          /// This file contains the data used to verify the permanence in the experiment.
          /// </summary>
          public string VerifyPermanence_InputFile { get; set; }
      }
  }

~~~

This defines an internal class named `ExerimentRequestMessage` within the `MyExperiment` namespace. This class is designed to represent a request for an experiment and implements the `IExerimentRequest` interface. The class includes several properties that hold information relevant to the experiment:

- **PopReceipt** : A string that helps manage the message in a queue, particularly for visibility and deletion.
- **ExperimentId** : A unique identifier for the experiment.
- **DecrementPermanence_InputFile** : The file path containing input data used to decrement permanence in the experiment.
- **Name** : The name of the experiment request.
- **Description** : A detailed description of the experiment.
- **MessageId** : A unique identifier for the message in the queue.
- **MessageReceipt** : Another string related to managing the message in the queue.
- **VerifyPermanence_InputFile** : The file path containing input data used to verify permanence in the experiment.

This class is intended for use within a system that manages experiment requests, likely involving a message queue to handle different experiment tasks.

~~~AzureStorageProvider.cs
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
~~~

The `AzureStorageProvider` class in the `MyExperiment` namespace is a concrete implementation of the `IStorageProvider` interface, designed to interact with Azure Storage services. The class provides methods for interacting with Azure Storage services, including deleting messages from queues, downloading files from blobs, and uploading results. It handles various exceptions and includes optional logging for operations.

- **`AzureStorageProvider(IConfigurationSection configSection)`**: Initializes the provider with configuration settings from an `IConfigurationSection`.

1. **`CommitRequestAsync(IExerimentRequest request)`**: It deletes a message from an Azure Storage Queue using the `MessageId` and `PopReceipt` from the `IExerimentRequest`. It handles exceptions for argument errors and Azure SDK request failures.

2. **`DownloadInputAsync(string fileName, string fileName1)`**: It downloads two files from Azure Blob Storage to a local directory. It creates a new directory for the downloaded files, deleting any existing directory with the same name.

3. **`ReceiveExperimentRequestAsync(CancellationToken token)`**: It receives and processes messages from an Azure Storage Queue. It deserializes the message body into an `IExerimentRequest` and handles exceptions related to JSON deserialization.

4. **`UploadExperimentResult(IExperimentResult result)`**: Intended for uploading experiment results.

5. **`UploadResultAsync(string experimentName, IExperimentResult result)`**: It uploads a result file to Azure Blob Storage. Also, optionally deletes existing blobs in the container and logs the successful upload.

## 7. Run Experiment

Queue Message
~~~
{
    "ExperimentId": "001",
    "Name": "Adapt Segments",
    "Description": "You can write your own description",
    "DecrementPermanence_InputFile": "json.json",
    "VerifyPermanence_InputFile": "testcases.json"
}
~~~

The JSON snippet represents a message format for an experiment request in a queue. It is used to serialize and deserialize experiment requests when interacting with an Azure Storage Queue. It contains:

- **`ExperimentId`**: `"001"` - A unique identifier for the experiment.
- **`Name`**: `"Adapt Segments"` - The name or title of the experiment.
- **`Description`**: `"You can write your own description"` - A textual description of the experiment, which can be customized.
- **`DecrementPermanence_InputFile`**: `"json.json"` - The file name of the input data used for the "decrement permanence" aspect of the experiment.
- **`VerifyPermanence_InputFile`**: `"testcases.json"` - The file name of the input data used for verifying permanence in the experiment.

## 8. Our output of the experiment

~~~
using Azure;
using Azure.Data.Tables;
using MyCloudProject.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyExperiment
{
    /// <summary>
    /// Represents the result of an experiment, including metadata and results.
    /// This class is designed to work with Azure Table Storage and implements
    /// the ITableEntity and IExperimentResult interfaces.
    /// </summary>
    public class ExperimentResult : ITableEntity, IExperimentResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentResult"/> class.
        /// </summary>
        /// <param name="partitionKey">The partition key for the Azure Table Storage entity.</param>
        /// <param name="rowKey">The row key for the Azure Table Storage entity.</param>
        public ExperimentResult(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        /// <summary>
        /// Gets or sets the partition key for the Azure Table Storage entity.
        /// </summary>
        public string PartitionKey { get; set; }

        /// <summary>
        /// Gets or sets the row key for the Azure Table Storage entity.
        /// </summary>
        public string RowKey { get; set; }

        /// <summary>
        /// Gets or sets the timestamp for when the entity was last modified.
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the ETag for the entity, used for concurrency control.
        /// </summary>
        public ETag ETag { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the experiment.
        /// </summary>
        public string ExperimentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the experiment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a description of the experiment.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the start time of the experiment in UTC.
        /// </summary>
        public DateTime? StartTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the end time of the experiment in UTC.
        /// </summary>
        public DateTime? EndTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the duration of the experiment in seconds.
        /// </summary>
        public long DurationSec { get; set; }

        /// <summary>
        /// Gets or sets the URL of the input file used for the experiment.
        /// </summary>
        public string InputFileUrl { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated list of output file URLs generated by the experiment.
        /// </summary>
        public string OutputFiles { get; set; }

        // Additional properties related to experiment results.

        /// <summary>
        /// Gets or sets the accuracy of the experiment results.
        /// </summary>
        public float Accuracy { get; set; }

        /// <summary>
        /// Gets or sets the duration of the experiment as a TimeSpan.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the pop receipt used to confirm that the result has been processed.
        /// </summary>
        public string PopReceipt { get; set; }

        /// <summary>
        /// Gets or sets the test case identifier associated with the experiment.
        /// </summary>
        public string testcase { get; set; }

        /// <summary>
        /// Gets or sets any comments or additional notes related to the experiment.
        /// </summary>
        public string Comments { get; set; }
    }
}

~~~

The `ExperimentResult` class represents the result of an experiment and is designed to work with Azure Table Storage. The class encapsulates both metadata and results of an experiment, including timing, file references, and performance metrics, and is tailored for use with Azure Table Storage. It implements the `ITableEntity` and `IExperimentResult` interfaces, providing a structure for storing and managing experiment data.

- **`PartitionKey`**: Identifies the partition within Azure Table Storage for this entity.
- **`RowKey`**: Identifies the unique row within the partition.
- **`Timestamp`**: The last modification timestamp for concurrency control.
- **`ETag`**: Used for concurrency control to ensure updates are based on the latest entity version.
- **`ExperimentId`**: Unique identifier for the experiment.
- **`Name`**: Name of the experiment.
- **`Description`**: Description or details of the experiment.
- **`StartTimeUtc`**: Start time of the experiment in UTC.
- **`EndTimeUtc`**: End time of the experiment in UTC.
- **`DurationSec`**: Duration of the experiment in seconds.
- **`InputFileUrl`**: URL of the input file used for the experiment.
- **`OutputFiles`**: Comma-separated list of URLs for output files generated by the experiment.
- **`Accuracy`**: Accuracy of the experiment results.
- **`Duration`**: Duration of the experiment represented as a `TimeSpan`.
- **`PopReceipt`**: Receipt used to confirm that the result has been processed.
- **`testcase`**: Identifier for the test case associated with the experiment.

~~~csharp
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
using Newtonsoft;
using NeoCortexApi.Entities;
using System.Xml;

namespace MyExperiment
{
    /// <summary>
    /// This class implements the ML experiment that will run in the cloud. This is refactored code from my SE project.
    /// </summary>
    public class Experiment : IExperiment
    {
        private IStorageProvider storageProvider; // Interface for storage operations
        private ILogger logger; // Logger for recording events
        private MyConfig config; // Configuration settings for the experiment
        Excel Excel = new Excel(); // Instance of Excel class for handling Excel operations

        /// <summary>
        /// Constructor to initialize the experiment with configuration, storage provider, and logger.
        /// </summary>
        /// <param name="configSection">Configuration section for the experiment.</param>
        /// <param name="storageProvider">Provider for storage operations.</param>
        /// <param name="log">Logger for the experiment.</param>
        public Experiment(IConfigurationSection configSection, IStorageProvider storageProvider, ILogger log)
        {
            this.storageProvider = storageProvider;
            this.logger = log;

            config = new MyConfig();
            configSection.Bind(config); // Bind configuration section to MyConfig instance
        }

        /// <summary>
        /// Runs the experiment asynchronously using the specified input files and folder.
        /// </summary>
        /// <param name="inputDataFolder">Folder containing input data files.</param>
        /// <param name="DecrementPermanence_InputFile">File for decrementing permanence input.</param>
        /// <param name="VerifyPermanence_InputFile">File for verifying permanence input.</param>
        /// <returns>Returns the result of the experiment as an IExperimentResult.</returns>
        public async Task<IExperimentResult> RunAsync(string inputDataFolder, string DecrementPermanence_InputFile, string VerifyPermanence_InputFile)
        {
            string excelName = "table_Result.xlsx"; // Name of the Excel file to be created
            string excelFilePath = Path.Combine(Directory.GetCurrentDirectory(), excelName); // Full path to the Excel file

            try
            {
                // Attempt to delete the existing Excel file if it exists
                if (File.Exists(excelFilePath))
                {
                    File.Delete(excelFilePath);
                    Console.WriteLine($"File {excelName} deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"File {excelName} does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting Excel file: {ex.Message}");
                logger?.LogError(ex, "Error deleting Excel file");
            }

            // Create an instance of testcases to run various unit tests
            var testcases = new testcases();

            // Create an ExperimentResult instance to store the output
            var res = new ExperimentResult(this.config.GroupId, null);

            // Run various unit tests and adapt segments as needed
            try
            {
                testcases.testcaseAdaptSegments_UnitTest_DecrementPermanenceIfInactivePresynapticCells(DecrementPermanence_InputFile);
                testcases.testcaseAdaptSegments_UnitTest_VerifyPermanenceChangeForPreviousCycle();
                testcases.testcaseAdaptSegments_UnitTest_VerifySegmentStateAfterMaxSynapsesPerSegment();
                testcases.testcaseAdaptSegments_UnitTest_VerifySegmentAndActiveSegmentStateAfterAdaptation();
                testcases.testcaseAdaptSegments_UnitTest_VerifyAdaptationWhenMaxSynapsesPerSegmentIsReachedAndExceeded();
                testcases.testcaseAdaptSegments_UnitTest_VerifySegmentDestructionWhenNoSynapseIsPresent();
                testcases.testcaseAdaptSegments_UnitTest_PreservesSynapses_ForSmallNegativePermanenceValues();
                testcases.testcaseAdaptSegments_UnitTest_VerifySynapseDestructionWithNegativePermanenceValuesAfterAdaptation();
                testcases.testcaseAdaptSegments_UnitTest_EnsureAdaptSegmentThrowsExceptionWhenDistalDendriteIsNull();
                testcases.testcaseAdaptSegments_UnitTest_CheckSynapseStateAfterAdaptatione();
                testcases.testcaseAdaptSegments_UnitTest_TestPermanenceIncrement_BoundaryConstraint();
                testcases.testcaseAdaptSegments_UnitTest_TestGetCells_ReturnsEmptyArrayForEmptyInput();
                testcases.testcaseAdaptSegments_UnitTest_TestGetCells_ValidInput_ReturnsExpectedCellArray();
                testcases.testcaseAdaptSegments_UnitTest_ComplexDoublePermanenceInput_MaxPermanenceReached();
                testcases.testcaseAdaptSegments_UnitTest_VerifySynapseRemovalOnMinimumPermanenceAdaptation();
                testcases.testcaseAdaptSegments_UnitTest_VerifySynapseDestructionOnLowPermanenceAdaptation();
                testcases.testcaseAdaptSegments_UnitTest_VerifyStayOfSynapseAfterSegmentAdaptation();
                testcases.testcaseAdaptSegments_UnitTest_TestInvalidArrayCells_WithInvalidArray_ThrowsIndexOutOfRangeException();
                testcases.testcaseAdaptSegments_UnitTest_TestNullArrayCells_ThrowsException();
                testcases.testcaseAdaptSegments_UnitTest_PreservesSynapses_ForVerySmallPermanenceValues();
                testcases.testcaseAdaptSegments_UnitTest_PreservesSynapses_ForVeryLargePermanenceValues();
                testcases.testcaseAdjustsSynapsePermanenceBasedOnPreviousActiveCells();
                testcases.testcasePreservesSynapses_ForVeryLargeNegativePermanenceValues();
                testcases.testcasePreservesSynapses_ForZeroPermanenceValues();
                testcases.testcaseVerify_Emptysegement();
                testcases.testcaseAdaptSegments_UnitTest_KillSegmentEvenIfOnlyoneSynapse_is_left();
                testcases.testcaseAdaptSegments_UnitTest_CheckIfSegmentSurvives();
                testcases.testcaseAdaptSegments_UnitTest_VerifyPermanenceBoundsAfterAdaptation(VerifyPermanence_InputFile);

                // Set the path of the output file in the result
                res.OutputFiles = excelFilePath;
                return res;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error occurred during experiment execution");
                throw;
            }
        }
    }
}
~~~

The `Experiment` class, which implements the `IExperiment` interface, is designed to run machine learning experiments in the cloud. It interacts with Azure services and uses configuration, storage, and logging facilities to manage and execute experiments. It initializes with configuration, a storage provider, and a logger.
- **RunAsync Method**: It deletes any existing Excel result file from the current directory.It runs a series of unit tests related to segment adaptation and permanence in an experimental setup using the provided input files. It creates an `ExperimentResult` object with the path to the generated Excel file, which contains the experiment results. 

The method captures and logs any errors encountered during execution, ensuring robust error handling and logging throughout the experiment.

## 9. About Adapt segments method

In the Hierarchical Temporal Memory (HTM) algorithm, the `AdaptSegments` method is crucial for updating synaptic permanence values in a distal dendrite segment based on the activity of presynaptic cells. The method starts by creating an empty list, `synapsesToDestroy`, to track synapses that need removal. It iterates over each synapse in the segment. Retrieves the current permanence value of each synapse. Checks if the corresponding presynaptic cell was active in the previous cycle:
     - **Active**: Increases the permanence by `permanenceIncrement`.
     - **Inactive**: Decreases the permanence by `permanenceDecrement`.
It ensures permanence values stay within the range [0, 1]. Values below 0 are set to 0, and those above 1 are capped at 1.

 **Destruction**: It compares permanence values to a threshold, `EPSILON`. Synapses with values below this threshold are added to `synapsesToDestroy`.It updates the permanence values of remaining synapses. It removes synapses in the `synapsesToDestroy` list. If a segment ends up with no synapses, it is deleted.
 
- CreateDistalSegment: It creates a new segment for a cell if it hasn't reached the maximum number of segments. If the maximum is reached, it removes the least recently used segment.
- DestroyDistalDendrite : It Deletes a specific segment and its synapses.
- LeastRecentlyUsedSegment : Finds the least recently used segment for a cell.
- NumSegments : Returns the number of distal dendrite segments for a cell or across cells.
This method and related functions ensure that segments and synapses are dynamically managed based on their activity, maintaining the efficiency and adaptability of the HTM model.

## 10. Azure Implementation

1. Resource Group (RG): The name of RG is 'RG-Team_NV'
   
![RG](https://github.com/nastayeen-abdulmajid/neocortexapi-project2024/blob/master/MyCloudProjectSample-new/Documentation/CC-images/RG.png)

2. Storage account: It is named as 'teamnv2024'.
3. Container registry: Named as 'teamnv'.
   
![Container registry](https://github.com/nastayeen-abdulmajid/neocortexapi-project2024/blob/master/MyCloudProjectSample-new/Documentation/CC-images/Container%20registry.png)

4. Container Instance: Named as 'teamnv'.
   
![Container instance](https://github.com/nastayeen-abdulmajid/neocortexapi-project2024/blob/master/MyCloudProjectSample-new/Documentation/CC-images/Container%20instance.png)

5. Docker Image: 'teamnv.azurecr.io/mycloudproject:v4'
   
![Docker image](https://github.com/nastayeen-abdulmajid/neocortexapi-project2024/blob/master/MyCloudProjectSample-new/Documentation/CC-images/docker%20image.png)

6. Blob type: A Block Blob in Azure Blob Storage is designed to store large amounts of unstructured data, such as text or binary data. Block blobs are the most commonly used type of blobs in Azure and are optimized for streaming and storing files.

## 11. How to run the experiment

1. Click on "Start" to initiate "teamnv" container instance on Azure.

![Container instance](https://github.com/nastayeen-abdulmajid/neocortexapi-project2024/blob/master/MyCloudProjectSample-new/Documentation/CC-images/Container%20instance.png)

2. Adding trigger messages to the queue by accessing the storage account "trigger-queue". The message is placed in the queue to trigger the execution of the experiment. The container instance will read this message, process the input files as specified, and run the experiment accordingly.
   
![queue message](https://github.com/nastayeen-abdulmajid/neocortexapi-project2024/blob/master/MyCloudProjectSample-new/Documentation/CC-images/Queues.png)

3. Check input file in storage
   
![input container](https://github.com/nastayeen-abdulmajid/neocortexapi-project2024/blob/master/MyCloudProjectSample-new/Documentation/CC-images/Input%20container.png)

4.  Logs monitoring by regularly checking the logs to track the experiment's progress and current status.
   
![logs](https://github.com/nastayeen-abdulmajid/neocortexapi-project2024/blob/master/MyCloudProjectSample-new/Documentation/CC-images/logs.png)

5. After the experiment completes, you can navigate to blob container within storage account to access "result-files".
   
![result](https://github.com/nastayeen-abdulmajid/neocortexapi-project2024/blob/master/MyCloudProjectSample-new/Documentation/CC-images/Output%20file.png)
   

## 12. Describing our Result

The result excel columns reperesents the following:
1. Timestamp : It shows when the experiment time starts.
2. Endtimetc : It shows when the experiment time ends.
3. ExperimentId : It represents the type of cases with Adapt segments of unit testing
4. DurationSec : It represents the amount of time taken to run the test cases
5. InputFileUrl : It respresents the path of external files used. Example: excel, json, etc.
6. TestCase : It shows the result of all test cases.
7. Comments : It represents additional information for each test case.

## 13. Consolidated Information of our components

| Types of cloud components | Name in our experiment | 
| ---------------           | ---------------        | 
| Container instance        | teamnv                 | 
| Queue storage             | trigger-queue          | 
| Input Blob container      | training-files         | 
| Output Blob container     | result-files           | 


## What is your experiment about

Describe here what your experiment is doing. Provide a reference to your SE project documentation (PDF)*)

1. What is the **input**?

2. What is the **output**?

3. What your algorithmas does? How ?

## How to run experiment

Describe Your Cloud Experiment based on the Input/Output you gave in the Previous Section.

**_Describe the Queue Json Message you used to trigger the experiment:_**  

~~~json
{
     ExperimentId = "123",
     InputFile = "https://beststudents2.blob.core.windows.net/documents2/daenet.mp4",
     .. // see project sample for more information 
};
~~~

- ExperimentId : Id of the experiment which is run  
- InputFile: The video file used for trainign process  

**_Describe your blob container registry:**  

what are the blob containers you used e.g.:  
- 'training_container' : for saving training dataset  
  - the file provided for training:  
  - zip, images, configs, ...  
- 'result_container' : saving output written file  
  - The file inside are result from the experiment, for example:  
  - **file Example** screenshot, file, code  


**_Describe the Result Table_**

 What is expected ?
 
 How many tables are there ? 
 
 How are they arranged ?
 
 What do the columns of the table mean ?
 
 Include a screenshot of your table from the portal or ASX (Azure Storage Explorer) in case the entity is too long, cut it in half or use another format
 
 - Column1 : explaination
 - Column2 : ...
Some columns are obligatory to the ITableEntities and don't need Explaination e.g. ETag, ...
 
# References:
1. https://www.researchgate.net/publication/261381455_An_overview_of_Hierarchical_Temporal_Memory_A_new_neocortex_algorithm
2. http://numenta.org/resources/HTM_CorticalLearningAlgorithms.pdf
3. https://www.tutorialspoint.com/software_testing_dictionary/failover_testing.htm
4. https://www.numenta.com/assets/pdf/whitepapers/hierarchical-temporal-memory-cortical-learning-algorithm-0.2.1-en.pdf
