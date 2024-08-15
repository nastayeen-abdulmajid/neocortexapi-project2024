using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyCloudProject.Common
{
    /// <summary>
    /// Defines the interface for an experiment that can be run with specific input data.
    /// </summary>
    public interface IExperiment
    {
        /// <summary>
        /// Runs the experiment asynchronously using the provided input data.
        /// </summary>
        /// <param name="inputDataFolder">The path to the folder containing the input data files for the experiment.</param>
        /// <param name="inputFile">The name of the primary input file that contains specific data needed for the experiment.</param>
        /// <param name="inputFile1">The name of the secondary input file that contains additional data required for the experiment.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains the result of the experiment.
        /// The result is an instance of <see cref="IExperimentResult"/>, which holds the outcome of the experiment.
        /// </returns>
        Task<IExperimentResult> RunAsync(string inputDataFolder, string inputFile, string inputFile1);
    }
}
