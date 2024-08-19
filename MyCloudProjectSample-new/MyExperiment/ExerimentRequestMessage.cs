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
        public string DecrementPermanenceInputFile { get; set; }

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
        
        public string VerifyPermanenceInputFile { get; set; }
    }
}
