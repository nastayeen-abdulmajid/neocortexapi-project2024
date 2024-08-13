using System;

namespace MyCloudProject.Common
{
    /// <summary>
    /// Defines the contract for the message request that will run your experiment.
    /// </summary>
    public interface IExerimentRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the experiment request.
        /// This identifier can be used to track or reference the experiment request.
        /// </summary>
        public string ExperimentId { get; set; }

        /// <summary>
        /// Gets or sets the URI of the file that contains the input arguments for the decrement permanence operation.
        /// This file should contain the data needed for processing the decrement permanence aspect of the experiment.
        /// </summary>
        public string DecrementPermanence_InputFile { get; set; }

        /// <summary>
        /// Gets or sets the URI of the file that contains the input arguments for the verify permanence operation.
        /// This file should contain the data needed for processing the verify permanence aspect of the experiment.
        /// </summary>
        public string VerifyPermanence_InputFile { get; set; }

        /// <summary>
        /// Gets or sets the name of the experiment request.
        /// This can be a descriptive name or label for the experiment, used for identification purposes.
        /// </summary>
        public string Name { get; set; }

        // Placeholder for additional properties
    }
}
