# Title of your SE Project - Azure Cloud Implementation

# Project : ML 22/23 - 7 Implement Unit Tests  for Adapt Segmants Method - Azure Cloud Implementation

# Table of contents

1. Introduction
2. Prerequisites
3. Project Structure
4. Configuration
5. Logging
6. Program Flow
7. Azure  integration
8. Processing
9. Testing
10. Deployment

## Introduction
Our project is a cloud-based system designed to execute experiments by processing tasks from Azure Storage Queues. It integrates with Azure Storage for handling experiment requests, downloading necessary input files, running the experiment, uploading the results, and managing the entire lifecycle of an experiment. The purpose of project was developed to automate the process of running experiments, particularly those related to machine learning. The goal is to make the experiment lifecycle efficient, scalable, and manageable in a cloud environment. 

# Technologies used
1. C# Programming language
2. Docker Desktop
3. Azure Cloud

# Project Architecture

The key components of the flow are as follows:
1. Github repository : A collaborative platform for version control, facilitating project source code management.
2. Visual Studio 2022 : A comprehensive IDE designed for coding, debugging, and testing software.
3. Docker desktop : A containerization tool that encapsulates the application within a Docker image for deployment on Azure.
4. Docker Image : A lightweight, portable package that includes all dependencies required to run the application.
5. Azure Container Registry : A service for storing and managing Docker images within Azure.
6. Azure Container Instances :  A service for running containerized applications on Azure, eliminating the need for infrastructure management.
7. Azure Storage : 
  b. Queue : A message queue that triggers the application's processing workflow upon receiving a queue message.
  c. Table : A storage solution for logging experiment execution data as part of the output.

 ## Prerequisites
1. Visual Studio 2022: We installed on your machine with the necessary workloads (ASP.NET, Azure, etc.). 
2. Azure Student Subscription: Active Azure account. 
3. Docker Desktop: Installed and running on your machine.

# Step 1: Setting Up Docker in our Adapt segments project

- Opening our existing project in Visual Studio 2022.
- Right-click on your project in the Solution Explorer. Select Add > Docker Support.
- Choosing the target OS (Linux is commonly used).
- Visual Studio will generated a Dockerfile in our project, which describes how your application will be containerized.

# Step 2: Build and Run Your Docker Container Locally
Building the Docker Image:

- Right-click the project in Solution Explorer. Choose Build or Rebuild to create the Docker image locally.
- Run the Docker Container Locally:
- Press F5 or click on the Run button to start the container.
This ensures everything is working fine locally before deploying to Azure.

# Step 3: Deploy to Azure

- In the Azure Portal, create a new Azure Container Registry(ACR)
- Log in to your Azure Container Registry.
- Tag and Push Docker Image to ACR.
- In the Azure Portal, create a new Web App with Docker support or use the Azure CLI
- Set the container settings to point to your image in ACR.


# Our input to the experiment
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
The code defines an internal class named `ExerimentRequestMessage` within the `MyExperiment` namespace. This class is designed to represent a request for an experiment and implements the `IExerimentRequest` interface. The class includes several properties that hold information relevant to the experiment:

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

### Constructor
- **`AzureStorageProvider(IConfigurationSection configSection)`**: Initializes the provider with configuration settings from an `IConfigurationSection`.

### Methods

1. **`CommitRequestAsync(IExerimentRequest request)`**: It deletes a message from an Azure Storage Queue using the `MessageId` and `PopReceipt` from the `IExerimentRequest`. It handles exceptions for argument errors and Azure SDK request failures.

2. **`DownloadInputAsync(string fileName, string fileName1)`**: It downloads two files from Azure Blob Storage to a local directory. It creates a new directory for the downloaded files, deleting any existing directory with the same name.

3. **`ReceiveExperimentRequestAsync(CancellationToken token)`**: It receives and processes messages from an Azure Storage Queue. It deserializes the message body into an `IExerimentRequest` and handles exceptions related to JSON deserialization.

4. **`UploadExperimentResult(IExperimentResult result)`**: Intended for uploading experiment results.

5. **`UploadResultAsync(string experimentName, IExperimentResult result)`**: It uploads a result file to Azure Blob Storage. Also, optionally deletes existing blobs in the container and logs the successful upload.


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

# Our output of the experiment

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
 
