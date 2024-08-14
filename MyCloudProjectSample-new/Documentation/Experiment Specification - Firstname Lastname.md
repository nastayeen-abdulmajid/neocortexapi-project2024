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

# Introduction
Our project is a cloud-based system designed to execute experiments by processing tasks from Azure Storage Queues. It integrates with Azure Storage for handling experiment requests, downloading necessary input files, running the experiment, uploading the results, and managing the entire lifecycle of an experiment. The purpose of project was developed to automate the process of running experiments, particularly those related to machine learning. The goal is to make the experiment lifecycle efficient, scalable, and manageable in a cloud environment. 

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


Use this file to describe your experiment.
This file is the whole documentation you need.
It should include images, best with relative path in Documentation. For Example "/pic/image.png"  
Do not paste code-snippets here as image. Use rather markdoown (MD) code documentation.
For example:

~~~csharp
public voiud MyFunction()
{
    Debug.WriteLine("this is a code sample");
}
~~~


## What is your experiment about

Describe here what your experiment is doing. Provide a reference to your SE project documentation (PDF)*)

1. What is the **input**?
2. What is the **output**?

3. What your algorithmas does? How ?...

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
 
