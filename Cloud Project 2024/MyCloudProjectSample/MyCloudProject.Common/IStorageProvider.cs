using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyCloudProject.Common
{
    /// <summary>
    /// Defines the contract for all storage operations related to the experiment process.
    /// </summary>
    public interface IStorageProvider
    {
        /// <summary>
        /// Receives the next message from the queue containing an experiment request.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> to monitor for cancellation requests.</param>
        /// <returns>
        /// An <see cref="IExerimentRequest"/> representing the next message in the queue, or <c>null</c> if there are no messages.
        /// </returns>
        Task<IExerimentRequest> ReceiveExperimentRequestAsync(CancellationToken token);

        /// <summary>
        /// Downloads an input file for training from a remote location.
        /// This file contains the necessary data required for running the experiment.
        /// </summary>
        /// <param name="fileName">The name of the input file to be downloaded from the remote (cloud) location.</param>
        /// <param name="fileName1">An additional file name or parameter if needed for the download operation.</param>
        /// <returns>
        /// The full local path where the file has been downloaded.
        /// </returns>
        /// <remarks>
        /// This operation corresponds to step 4 in the architecture diagram.
        /// </remarks>
        Task<string> DownloadInputAsync(string fileName, string fileName1);

        /// <summary>
        /// Uploads the results of the experiment to a remote location.
        /// This involves storing the results in the cloud or any other storage system.
        /// </summary>
        /// <param name="experimentName">The name of the experiment, used to identify the file at the remote location.</param>
        /// <param name="result">The result of the experiment to be uploaded.</param>
        /// <remarks>
        /// This operation corresponds to step 5 (in reverse) in the architecture diagram.
        /// </remarks>
        Task UploadResultAsync(string experimentName, IExperimentResult result);

        /// <summary>
        /// Commits the message request to ensure it is removed from the queue.
        /// This method should be called after processing the experiment request to remove it from the queue.
        /// </summary>
        /// <param name="request">The experiment request received from <see cref="ReceiveExperimentRequestAsync"/> that needs to be committed.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task CommitRequestAsync(IExerimentRequest request);
    }
}
