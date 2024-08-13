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
