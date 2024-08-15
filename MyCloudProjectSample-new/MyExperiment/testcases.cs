using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCloudProject.Common;
using NeoCortexApi.Entities;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using System.Text.Json;
using System.Threading;
using System.Xml;



namespace MyExperiment 
{
  
    public class testcases
    {
        AdaptSegments adaptSegments = new AdaptSegments();
        Excel Excel = new Excel();

        /// <summary>
        /// This method is a unit test for verifying the permanence change for a previous cycle in the AdaptSegments functionality.
        /// It measures the execution time of the test and logs the result in an `ExperimentResult` object.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_VerifyPermanenceChangeForPreviousCycle()
        {
            // Create a new instance of ExperimentResult to store the results of this test
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the method under test and capture the result
            int result1 = adaptSegments.AdaptSegments_UnitTest_VerifyPermanenceChangeForPreviousCycle();

            // Record the end time of the test
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Set the experiment ID and calculate the duration of the test
            res.ExperimentId = "AdaptSegments_UnitTest_VerifyPermanenceChangeForPreviousCycle";
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Check the result of the test and log the outcome
            if (result1 == 1)
            {
                // Test passed
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifyPermanenceChangeForPreviousCycle passed");
                res.testcase = "Passed";
                res.Comments = "Test passed successfully.";
            }
            else if (result1 == 0)
            {
                // Test failed
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifyPermanenceChangeForPreviousCycle failed");
                res.testcase = "Failed";
                res.Comments = "Test failed.";
            }

            // No external input file was needed for this test
            res.InputFileUrl = "No External Input File needed";

            // Write the results of the test to an Excel file
            Excel.WriteDataToExcel(res);
        }



        /// <summary>
        /// Executes a unit test case to verify the state of segments after reaching the maximum number of synapses per segment.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_VerifySegmentStateAfterMaxSynapsesPerSegment()
        {
            // Create an instance of ExperimentResult to hold the details of the test execution
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test case execution
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the method under test and store the result
            int result1 = adaptSegments.AdaptSegments_UnitTest_VerifySegmentStateAfterMaxSynapsesPerSegment();

            // Record the end time of the test case execution
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Assign a unique experiment identifier for this test case
            res.ExperimentId = "AdaptSegments_UnitTest_VerifySegmentStateAfterMaxSynapsesPerSegment";

            // Calculate and store the duration of the test case execution
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Determine the test result and set appropriate messages and status
            if (result1 == 1)
            {
                // Test passed
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifySegmentStateAfterMaxSynapsesPerSegment Test Case Passed");
                res.testcase = "Passed";
                res.Comments = "Test passed successfully.";
            }
            else if (result1 == 0)
            {
                // Test failed
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifySegmentStateAfterMaxSynapsesPerSegment Test Case Failed");
                res.testcase = "Failed";
                res.Comments = "Test failed.";
            }

            // Specify that no external input file was required for this test case
            res.InputFileUrl = "No External Input File needed";

            // Write the test result data to an Excel file
            Excel.WriteDataToExcel(res);
        }


        /// <summary>
        /// Unit test method to verify the segment and active segment state after adaptation.
        /// This method executes the adaptation process, records the result, and logs the test outcome.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_VerifySegmentAndActiveSegmentStateAfterAdaptation()
        {
            // Create an instance of ExperimentResult to record test execution details.
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test.
            res.StartTimeUtc = DateTime.UtcNow;

            // Execute the adaptation and capture the result.
            int result1 = adaptSegments.AdaptSegments_UnitTest_VerifySegmentAndActiveSegmentStateAfterAdaptation();

            // Record the end time of the test.
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Set the experiment ID to identify the test case.
            res.ExperimentId = "AdaptSegments_UnitTest_VerifySegmentAndActiveSegmentStateAfterAdaptation";

            // Calculate and record the duration of the test in seconds.
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Determine the test outcome based on the result and log appropriate messages.
            if (result1 == 1)
            {
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifySegmentAndActiveSegmentStateAfterAdaptation Test Case Passed");
                res.testcase = "Passed";
                res.Comments = "Test passed successfully.";
            }
            else if (result1 == 0)
            {
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifySegmentAndActiveSegmentStateAfterAdaptation Test Case Failed");
                res.testcase = "Failed";
                res.Comments = "Test failed.";
            }

            // No external input file was needed for this test case.
            res.InputFileUrl = "No External Input File needed";

            // Write the test results to an Excel file for reporting.
            Excel.WriteDataToExcel(res);
        }



        /// <summary>
        /// Executes the unit test to verify the adaptation of segments when the maximum number of synapses per segment is reached and exceeded.
        /// Logs the results and records them in an Excel file.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_VerifyAdaptationWhenMaxSynapsesPerSegmentIsReachedAndExceeded()
        {
            // Initialize the ExperimentResult object to record the test results
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the method under test and capture the results
            (int result1, string result2) = adaptSegments.AdaptSegments_UnitTest_VerifyAdaptationWhenMaxSynapsesPerSegmentIsReachedAndExceeded();

            // Record the end time of the test
            res.EndTimeUtc = DateTime.UtcNow;

            // Set additional properties for the ExperimentResult
            res.ExperimentId = "AdaptSegments_UnitTest_VerifyAdaptationWhenMaxSynapsesPerSegmentIsReachedAndExceeded";

            // Record the end time of the test.
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Calculate the duration of the test in seconds
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Determine the outcome of the test based on the result1 value
            if (result1 == 1)
            {
                // Test passed
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifyAdaptationWhenMaxSynapsesPerSegmentIsReachedAndExceeded Test Case Passed");
                res.testcase = "Passed";
                res.Comments = result2;
            }
            else if (result1 == 0)
            {
                // Test failed
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifyAdaptationWhenMaxSynapsesPerSegmentIsReachedAndExceeded Test Case Failed");
                res.testcase = "Failed";
                res.Comments = result2;
            }

            // Specify that no external input file is needed for this test case
            res.InputFileUrl = "No External Input File needed";

            // Write the results to an Excel file for record-keeping
            Excel.WriteDataToExcel(res);
        }


        /// <summary>
        /// Unit test to verify segment destruction when no synapse is present.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_VerifySegmentDestructionWhenNoSynapseIsPresent()
        {
            // Create an instance of ExperimentResult to log test details
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the method under test and store the result
            int result1 = adaptSegments.AdaptSegments_UnitTest_VerifySegmentDestructionWhenNoSynapseIsPresent();

            // Record the end time of the test
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Set the experiment ID for the result
            res.ExperimentId = "AdaptSegments_UnitTest_VerifySegmentDestructionWhenNoSynapseIsPresent";

            // Calculate and set the duration of the test in seconds
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Check the result of the test and log the outcome
            if (result1 == 1)
            {
                // If result1 is 1, the test case passed
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifySegmentDestructionWhenNoSynapseIsPresent Test Case Passed");
                res.testcase = "Passed";
                res.Comments = "Test passed successfully.";
            }
            else if (result1 == 0)
            {
                // If result1 is 0, the test case failed
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifySegmentDestructionWhenNoSynapseIsPresent Test Case Failed");
                res.testcase = "Failed";
                res.Comments = "Test failed.";
            }

            // Set the input file URL to indicate no external input file was needed for this test
            res.InputFileUrl = "No External Input File needed";

            // Write the result data to an Excel file
            Excel.WriteDataToExcel(res);
        }

        /// <summary>
        /// Executes a unit test for the method AdaptSegments_UnitTest_PreservesSynapses_ForSmallNegativePermanenceValues.
        /// This test case verifies that small negative permanence values are handled correctly and that synapses are preserved as expected.
        /// The results of the test are logged and saved to an Excel file.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_PreservesSynapses_ForSmallNegativePermanenceValues()
        {
            // Create a new instance of ExperimentResult to store the test result details.
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test.
            res.StartTimeUtc = DateTime.UtcNow;

            // Execute the unit test and capture the results.
            // The AdaptSegments_UnitTest_PreservesSynapses_ForSmallNegativePermanenceValues method should return a tuple with an integer and a string.
            (int result1, string result2) = adaptSegments.AdaptSegments_UnitTest_PreservesSynapses_ForSmallNegativePermanenceValues();

            // Record the end time of the test.
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Assign an experiment ID to the result for identification.
            res.ExperimentId = "AdaptSegments_UnitTest_PreservesSynapses_ForSmallNegativePermanenceValues";

            // Calculate the duration of the test execution.
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Check the result of the test and log the appropriate message.
            if (result1 == 1)
            {
                // Test passed.
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_PreservesSynapses_ForSmallNegativePermanenceValues Test Case Passed");
                res.testcase = "Passed";
                res.Comments = result2; // Capture any additional comments from the test result.
            }
            else if (result1 == 0)
            {
                // Test failed.
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_PreservesSynapses_ForSmallNegativePermanenceValues Test Case Failed");
                res.testcase = "Failed";
                res.Comments = result2; // Capture any additional comments from the test result.
            }

            // Set the input file URL. In this case, no external input file is needed.
            res.InputFileUrl = "No External Input File needed";

            // Write the test result data to an Excel file.
            Excel.WriteDataToExcel(res);
        }



        /// <summary>
        /// Unit test for verifying the behavior of the AdaptSegments method in the presence of negative permanence values
        /// after adaptation. This test checks if synapses are correctly destroyed when negative permanence values are present.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_VerifySynapseDestructionWithNegativePermanenceValuesAfterAdaptation()
        {
            // Create a new instance of ExperimentResult to store test results and metadata.
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test execution.
            res.StartTimeUtc = DateTime.UtcNow;

            // Execute the AdaptSegments method and capture the result.
            // 'result1' indicates the success of the test (1 for pass, 0 for fail).
            // 'result2' contains additional information or error messages.
            (int result1, string result2) = adaptSegments.AdaptSegments_UnitTest_VerifySynapseDestructionWithNegativePermanenceValuesAfterAdaptation();

            // Record the end time of the test execution.
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Set the unique experiment ID for this test case.
            res.ExperimentId = "AdaptSegments_UnitTest_VerifySynapseDestructionWithNegativePermanenceValuesAfterAdaptation";

            // Calculate the duration of the test in seconds.
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Evaluate the test result and update the ExperimentResult object accordingly.
            if (result1 == 1)
            {
                // If the result indicates success, log a success message and mark the test case as passed.
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifySynapseDestructionWithNegativePermanenceValuesAfterAdaptation Test Case Passed");
                res.testcase = "Passed";
                res.Comments = result2; // Include any additional comments from the result.
            }
            else if (result1 == 0)
            {
                // If the result indicates failure, log a failure message and mark the test case as failed.
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifySynapseDestructionWithNegativePermanenceValuesAfterAdaptation Test Case Failed");
                res.testcase = "Failed";
                res.Comments = result2; // Include any additional comments from the result.
            }

            // Set the input file URL for the test case. In this case, no external input file is needed.
            res.InputFileUrl = "No External Input File needed";

            // Write the results to an Excel file for record-keeping and analysis.
            Excel.WriteDataToExcel(res);
        }


        /// <summary>
        /// This method tests the behavior of the `adaptSegments` method to ensure that an exception is thrown
        /// when the distal dendrite is null. It captures the results of the test, including the duration and
        /// any relevant comments, and then logs these results to an Excel file.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_EnsureAdaptSegmentThrowsExceptionWhenDistalDendriteIsNull()
        {
            // Create an instance of ExperimentResult to log test results
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the method under test and capture the results
            (int result1, string result2) = adaptSegments.AdaptSegments_UnitTest_EnsureAdaptSegmentThrowsExceptionWhenDistalDendriteIsNull();

            // Record the timestamp of the test execution
            res.Timestamp = DateTime.UtcNow;

            // Record the end time of the test
            res.EndTimeUtc = DateTime.UtcNow;

            // Assign a unique identifier to this experiment
            res.ExperimentId = "AdaptSegments_UnitTest_EnsureAdaptSegmentThrowsExceptionWhenDistalDendriteIsNull";

            // Calculate the duration of the test
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Determine if the test passed or failed based on the result
            if (result1 == 1)
            {
                // Test passed
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_EnsureAdaptSegmentThrowsExceptionWhenDistalDendriteIsNull Test Case Passed");
                res.testcase = "Passed";
                res.Comments = result2;
            }
            else if (result1 == 0)
            {
                // Test failed
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_EnsureAdaptSegmentThrowsExceptionWhenDistalDendriteIsNull Test Case Failed");
                res.testcase = "Failed";
                res.Comments = result2;
            }

            // Specify that no external input file was needed for this test
            res.InputFileUrl = "No External Input File needed";

            // Write the test result data to an Excel file
            Excel.WriteDataToExcel(res);
        }


        /// <summary>
        /// This method executes a unit test case to check the state of synapses after adaptation.
        /// It records the start and end times, calculates the duration, and logs the result.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_CheckSynapseStateAfterAdaptatione()
        {
            // Create an instance of ExperimentResult to store test results and metadata.
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test.
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the method under test and capture its results.
            // The method 'adaptSegments.AdaptSegments_UnitTest_CheckSynapseStateAfterAdaptatione()' 
            // is expected to return a tuple containing an integer result and a string message.
            (int result1, string result2) = adaptSegments.AdaptSegments_UnitTest_CheckSynapseStateAfterAdaptatione();

            // Record the timestamp of when the test result was obtained.
            res.Timestamp = DateTime.UtcNow;

            // Record the end time of the test.
            res.EndTimeUtc = DateTime.UtcNow;

            // Assign a unique identifier for the test case.
            res.ExperimentId = "AdaptSegments_UnitTest_CheckSynapseStateAfterAdaptatione";

            // Calculate the duration of the test execution in seconds.
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Determine the outcome of the test based on the result1 value.
            if (result1 == 1)
            {
                // Log a message indicating the test case passed and update the result status.
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_CheckSynapseStateAfterAdaptatione Test Case Passed");
                res.testcase = "Passed";
                res.Comments = result2;
            }
            else if (result1 == 0)
            {
                // Log a message indicating the test case failed and update the result status.
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_CheckSynapseStateAfterAdaptatione Test Case Failed");
                res.testcase = "Failed";
                res.Comments = result2;
            }

            // Set the input file URL to indicate that no external input file was needed for this test.
            res.InputFileUrl = "No External Input File needed";

            // Write the experiment result data to an Excel file for record-keeping.
            Excel.WriteDataToExcel(res);
        }

        /// <summary>
        /// This method executes a unit test for the `AdaptSegments` functionality, specifically
        /// for the test case that evaluates the permanence increment with boundary constraints.
        /// It measures the time taken for the test, logs the results, and writes the results to an Excel file.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_TestPermanenceIncrement_BoundaryConstraint()
        {
            // Create a new ExperimentResult object to store the results of the test.
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the experiment.
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the AdaptSegments method and retrieve the results.
            // result1 indicates if the test passed (1) or failed (0).
            // result2 contains any additional comments or messages.
            (int result1, string result2) = adaptSegments.AdaptSegments_UnitTest_TestPermanenceIncrement_BoundaryConstraint();

            // Record the timestamp when the results are processed.
            res.Timestamp = DateTime.UtcNow;

            // Record the end time of the experiment.
            res.EndTimeUtc = DateTime.UtcNow;

            // Assign an identifier to the experiment result for tracking purposes.
            res.ExperimentId = "AdaptSegments_UnitTest_CheckSynapseStateAfterAdaptatione";

            // Calculate the duration of the test execution in seconds.
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Determine the test result based on the value of result1.
            if (result1 == 1)
            {
                // If result1 is 1, the test passed.
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_TestPermanenceIncrement_BoundaryConstraint Test Case Passed");
                res.testcase = "Passed";
                res.Comments = result2; // Add any additional comments from result2.
            }
            else if (result1 == 0)
            {
                // If result1 is 0, the test failed.
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_TestPermanenceIncrement_BoundaryConstraint Test Case Failed");
                res.testcase = "Failed";
                res.Comments = result2; // Add any additional comments from result2.
            }

            // Specify that no external input file was needed for this test.
            res.InputFileUrl = "No External Input File needed";

            // Write the results of the test to an Excel file for record-keeping.
            Excel.WriteDataToExcel(res);
        }


        /// <summary>
        /// This unit test method evaluates the functionality of the AdaptSegments_UnitTest_TestGetCells_ReturnsEmptyArrayForEmptyInput method.
        /// It measures the execution time and logs the results, including the outcome and any comments generated during the test.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_TestGetCells_ReturnsEmptyArrayForEmptyInput()
        {
            // Create a new instance of ExperimentResult to record the details of this test case.
            ExperimentResult res = new ExperimentResult(null, null);

            // Set the start time for the experiment.
            res.StartTimeUtc = DateTime.UtcNow;

            // Execute the method under test and capture its results.
            (int result1, string result2) = adaptSegments.AdaptSegments_UnitTest_TestGetCells_ReturnsEmptyArrayForEmptyInput();

            // Record the end time for the experiment.
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Set a unique identifier for this experiment result.
            res.ExperimentId = "AdaptSegments_UnitTest_TestGetCells_ReturnsEmptyArrayForEmptyInput";

            // Calculate the duration of the test execution in seconds.
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Determine the outcome of the test based on the result1 value.
            if (result1 == 1)
            {
                // If result1 equals 1, the test case is considered passed.
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_TestGetCells_ReturnsEmptyArrayForEmptyInput Test Case Passed");
                res.testcase = "Passed";
                res.Comments = result2; // Capture any comments or additional information from the test.
            }
            else if (result1 == 0)
            {
                // If result1 equals 0, the test case is considered failed.
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_TestGetCells_ReturnsEmptyArrayForEmptyInput Test Case Failed");
                res.testcase = "Failed";
                res.Comments = result2; // Capture any comments or additional information from the test.
            }

            // Indicate that no external input file was needed for this test case.
            res.InputFileUrl = "No External Input File needed";

            // Write the experiment results to an Excel file for documentation and review.
            Excel.WriteDataToExcel(res);
        }

        /// <summary>
        /// Unit test for the AdaptSegments method to validate the cell array output with valid input.
        /// This test ensures that the method returns the expected results and logs the outcome.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_TestGetCells_ValidInput_ReturnsExpectedCellArray()
        {
            // Initialize a new ExperimentResult object to track the test results and timing.
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test.
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the method under test and capture the results.
            // The method 'AdaptSegments_UnitTest_TestGetCells_ValidInput_ReturnsExpectedCellArray' should
            // return a tuple where result1 is the status code and result2 is a message or comment.
            (int result1, string result2) = adaptSegments.AdaptSegments_UnitTest_TestGetCells_ValidInput_ReturnsExpectedCellArray();

            // Record the end time of the test.
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Set the experiment ID based on the test case name.
            res.ExperimentId = "AdaptSegments_UnitTest_TestGetCells_ValidInput_ReturnsExpectedCellArray";

            // Calculate the duration of the test execution.
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Evaluate the test results based on the returned status code (result1).
            if (result1 == 1)
            {
                // Test case passed. Log the success message and update the test result status.
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_TestGetCells_ValidInput_ReturnsExpectedCellArray Test Case Passed");
                res.testcase = "Passed";
                res.Comments = result2;  // Add any additional comments or messages from the method.
            }
            else if (result1 == 0)
            {
                // Test case failed. Log the failure message and update the test result status.
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_TestGetCells_ValidInput_ReturnsExpectedCellArray Test Case Failed");
                res.testcase = "Failed";
                res.Comments = result2;  // Add any additional comments or messages from the method.
            }

            // Specify that no external input file is needed for this test case.
            res.InputFileUrl = "No External Input File needed";

            // Write the test results to an Excel file for record-keeping and analysis.
            Excel.WriteDataToExcel(res);
        }

        /// <summary>
        /// Unit test for the AdaptSegments method to check if maximum permanence is reached with complex double permanence input.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_ComplexDoublePermanenceInput_MaxPermanenceReached()
        {
            // Initialize an instance of ExperimentResult to capture the test execution details
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test
            res.StartTimeUtc = DateTime.UtcNow;

            // Execute the test case and capture the result
            int result1 = adaptSegments.AdaptSegments_UnitTest_ComplexDoublePermanenceInput_MaxPermanenceReached();

            // Record the timestamp when the test finished
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Set the experiment ID for this test case
            res.ExperimentId = "AdaptSegments_UnitTest_ComplexDoublePermanenceInput_MaxPermanenceReached";

            // Calculate the duration of the test in seconds
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Log the result of the test case to the console
            if (result1 == 1)
            {
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_ComplexDoublePermanenceInput_MaxPermanenceReached Test Case Passed");
                res.testcase = "Passed";
                res.Comments = "Test passed successfully.";
            }
            else if (result1 == 0)
            {
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_ComplexDoublePermanenceInput_MaxPermanenceReached Test Case Failed");
                res.testcase = "Failed";
                res.Comments = "Test failed.";
            }

            // Indicate that no external input file was needed for this test
            res.InputFileUrl = "No External Input File needed";

            // Write the test result data to an Excel file
            Excel.WriteDataToExcel(res);
        }


        /// <summary>
        /// Unit test for verifying the removal of synapses on minimum permanence adaptation in segments.
        /// This test case checks if the adaptation process correctly removes synapses that fall below the minimum permanence threshold.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_VerifySynapseRemovalOnMinimumPermanenceAdaptation()
        {
            // Create a new instance of ExperimentResult to track the details of the test execution
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test
            res.StartTimeUtc = DateTime.UtcNow;

            // Execute the method to test and capture the results
            // adaptSegments.AdaptSegments_UnitTest_VerifySynapseRemovalOnMinimumPermanenceAdaptation() should return a tuple
            (int result1, string result2) = adaptSegments.AdaptSegments_UnitTest_VerifySynapseRemovalOnMinimumPermanenceAdaptation();

            // Record the end time of the test
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Set the experiment ID to identify this specific test case
            res.ExperimentId = "AdaptSegments_UnitTest_VerifySynapseRemovalOnMinimumPermanenceAdaptation";

            // Calculate the duration of the test in seconds
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Check the result of the test and set the appropriate status and comments
            if (result1 == 1)
            {
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifySynapseRemovalOnMinimumPermanenceAdaptation Test Case Passed");
                res.testcase = "Passed"; // Mark the test case as Passed
                res.Comments = result2;  // Add any comments or messages from the test result
            }
            else if (result1 == 0)
            {
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifySynapseRemovalOnMinimumPermanenceAdaptation Test Case Failed");
                res.testcase = "Failed"; // Mark the test case as Failed
                res.Comments = result2;  // Add any comments or messages from the test result
            }

            // Indicate that no external input file was required for this test case
            res.InputFileUrl = "No External Input File needed";

            // Write the test results to an Excel file for further analysis or record-keeping
            Excel.WriteDataToExcel(res);
        }


        /// <summary>
        /// Unit test for verifying the destruction of synapses on low permanence adaptation.
        /// This test case checks if synapses are properly destroyed when the permanence value is low.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_VerifySynapseDestructionOnLowPermanenceAdaptation()
        {
            // Create an instance of ExperimentResult to store test results and metadata.
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test case.
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the method under test and capture the results.
            // 'adaptSegments.AdaptSegments_UnitTest_VerifySynapseDestructionOnLowPermanenceAdaptation()' 
            // is assumed to return a tuple where:
            // - result1 indicates the success status (1 for pass, 0 for fail).
            // - result2 contains a descriptive message about the test result.
            (int result1, string result2) = adaptSegments.AdaptSegments_UnitTest_VerifySynapseDestructionOnLowPermanenceAdaptation();

            // Record the end time of the test case.
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Set the experiment ID to uniquely identify this test case.
            res.ExperimentId = "AdaptSegments_UnitTest_VerifySynapseDestructionOnLowPermanenceAdaptation";

            // Calculate the duration of the test case in seconds.
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Determine the outcome of the test case based on the result1 value.
            if (result1 == 1)
            {
                // Test case passed.
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifySynapseDestructionOnLowPermanenceAdaptation Test Case Passed");
                res.testcase = "Passed";
                res.Comments = result2;
            }
            else if (result1 == 0)
            {
                // Test case failed.
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifySynapseDestructionOnLowPermanenceAdaptation Test Case Failed");
                res.testcase = "Failed";
                res.Comments = result2;
            }

            // Specify that no external input file was needed for this test case.
            res.InputFileUrl = "No External Input File needed";

            // Write the test results to an Excel file using the Excel.WriteDataToExcel method.
            Excel.WriteDataToExcel(res);
        }

        /// <summary>
        /// Unit test for verifying the behavior of segment adaptation in the Synapse system.
        /// This test checks if the system's state remains consistent after adapting segments.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_VerifyStayOfSynapseAfterSegmentAdaptation()
        {
            // Create an instance of ExperimentResult to store test execution details
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test execution
            res.StartTimeUtc = DateTime.UtcNow;

            // Execute the segment adaptation method and capture the results
            (int result1, string result2) = adaptSegments.AdaptSegments_UnitTest_VerifyStayOfSynapseAfterSegmentAdaptation();

            // Record the end time of the test execution
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Set the experiment ID for tracking purposes
            res.ExperimentId = "AdaptSegments_UnitTest_VerifyStayOfSynapseAfterSegmentAdaptation";

            // Calculate the duration of the test execution
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Determine the outcome of the test based on the result
            if (result1 == 1)
            {
                // Log and record a passed test case
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifyStayOfSynapseAfterSegmentAdaptation Test Case Passed");
                res.testcase = "Passed";
                res.Comments = result2; // Add any comments or details from the result
            }
            else if (result1 == 0)
            {
                // Log and record a failed test case
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifyStayOfSynapseAfterSegmentAdaptation Test Case Failed");
                res.testcase = "Failed";
                res.Comments = result2; // Add any comments or details from the result
            }

            // Specify that no external input file was needed for this test
            res.InputFileUrl = "No External Input File needed";

            // Write the results to an Excel file for record-keeping
            Excel.WriteDataToExcel(res);
        }


        /// <summary>
        /// Unit test method for the `AdaptSegments` function to validate handling of invalid array cells.
        /// It checks that an `IndexOutOfRangeException` is thrown when an invalid array is used.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_TestInvalidArrayCells_WithInvalidArray_ThrowsIndexOutOfRangeException()
        {
            // Create an instance of ExperimentResult to log the test results.
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test.
            res.StartTimeUtc = DateTime.UtcNow;

            // Execute the method being tested and capture the results.
            (int result1, string result2) = adaptSegments.AdaptSegments_UnitTest_TestInvalidArrayCells_WithInvalidArray_ThrowsIndexOutOfRangeException();

            // Record the timestamp after the test execution.
            res.Timestamp = DateTime.UtcNow;

            // Record the end time of the test.
            res.EndTimeUtc = DateTime.UtcNow;

            // Set the unique identifier for the test case.
            res.ExperimentId = "AdaptSegments_UnitTest_TestInvalidArrayCells_WithInvalidArray_ThrowsIndexOutOfRangeException";

            // Calculate the elapsed time of the test execution.
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;

            // Set the duration of the test in seconds.
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Check the result of the test and log appropriate messages.
            if (result1 == 1)
            {
                // Test case passed.
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_TestInvalidArrayCells_WithInvalidArray_ThrowsIndexOutOfRangeException Test Case Passed");
                res.testcase = "Passed";
                res.Comments = result2;
            }
            else if (result1 == 0)
            {
                // Test case failed.
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_TestInvalidArrayCells_WithInvalidArray_ThrowsIndexOutOfRangeException Test Case Failed");
                res.testcase = "Failed";
                res.Comments = result2;
            }

            // Set the input file URL (if any, but here it's noted as not needed).
            res.InputFileUrl = "No External Input File needed";

            // Write the results to an Excel file for documentation.
            Excel.WriteDataToExcel(res);
        }


        /// <summary>
        /// Unit test for the <see cref="AdaptSegments"/> method to check if it throws an exception when 
        /// provided with null array cells. This test case logs the results and writes them to an Excel file.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_TestNullArrayCells_ThrowsException()
        {
            // Create a new ExperimentResult object to log the results of the test case
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test case
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the method under test and capture the results
            (int result1, string result2) = adaptSegments.AdaptSegments_UnitTest_TestNullArrayCells_ThrowsException();

            // Record the end time of the test case
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Set the experiment ID to identify this specific test case
            res.ExperimentId = "AdaptSegments_UnitTest_TestNullArrayCells_ThrowsException";

            // Calculate the duration of the test case execution
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Determine the result of the test case and log appropriate messages
            if (result1 == 1)
            {
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_TestNullArrayCells_ThrowsException Test Case Passed");
                res.testcase = "Passed";
                res.Comments = result2;
            }
            else if (result1 == 0)
            {
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_TestNullArrayCells_ThrowsException Test Case Failed");
                res.testcase = "Failed";
                res.Comments = result2;
            }

            // Indicate that no external input file was needed for this test case
            res.InputFileUrl = "No External Input File needed";

            // Write the result data to an Excel file
            Excel.WriteDataToExcel(res);
        }

        /// <summary>
        /// This method executes the unit test for the `AdaptSegments` functionality, specifically checking if the method
        /// `AdaptSegments_UnitTest_PreservesSynapses_ForVerySmallPermanenceValues` correctly handles very small permanence values.
        /// It measures and logs the test execution time and outcome, and writes the result to an Excel file.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_PreservesSynapses_ForVerySmallPermanenceValues()
        {
            // Create a new instance of ExperimentResult to store details about the test run
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test
            res.StartTimeUtc = DateTime.UtcNow;

            // Execute the unit test and capture the results
            (int result1, string result2) = adaptSegments.AdaptSegments_UnitTest_PreservesSynapses_ForVerySmallPermanenceValues();

            // Record the end time of the test
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Set the Experiment ID for this test case
            res.ExperimentId = "AdaptSegments_UnitTest_PreservesSynapses_ForVerySmallPermanenceValues";

            // Calculate the duration of the test in seconds
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Check the result of the test and log the outcome
            if (result1 == 1)
            {
                // Test passed
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_PreservesSynapses_ForVerySmallPermanenceValues Test Case Passed");
                res.testcase = "Passed";
                res.Comments = result2;
            }
            else if (result1 == 0)
            {
                // Test failed
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_PreservesSynapses_ForVerySmallPermanenceValues Test Case Failed");
                res.testcase = "Failed";
                res.Comments = result2;
            }

            // Set the URL for external input files (none needed for this test)
            res.InputFileUrl = "No External Input File needed";

            // Write the results to an Excel file
            Excel.WriteDataToExcel(res);
        }

        /// <summary>
        /// Executes a unit test for the `AdaptSegments` method to verify its behavior with very large permanence values.
        /// It measures the execution time and logs the result of the test.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_PreservesSynapses_ForVeryLargePermanenceValues()
        {
            // Create an instance of ExperimentResult to store test details and results
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the method to be tested and store the result
            int result1 = adaptSegments.AdaptSegments_UnitTest_PreservesSynapses_ForVeryLargePermanenceValues();

            // Record the end time of the test
            res.StartTimeUtc = DateTime.UtcNow;

            // Generate a unique identifier for this test case
            res.ExperimentId = "AdaptSegments_UnitTest_PreservesSynapses_ForVeryLargePermanenceValues";

            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Calculate the duration of the test
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Determine the test result based on the method's return value
            if (result1 == 1)
            {
                // Test passed
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_PreservesSynapses_ForVeryLargePermanenceValues Test Case Passed");
                res.testcase = "Passed";
                res.Comments = "Test passed successfully.";
            }
            else if (result1 == 0)
            {
                // Test failed
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_PreservesSynapses_ForVeryLargePermanenceValues Test Case Failed");
                res.testcase = "Failed";
                res.Comments = "Test failed.";
            }

            // Note that no external input file is needed for this test
            res.InputFileUrl = "No External Input File needed";

            // Write the results to an Excel file for further analysis
            Excel.WriteDataToExcel(res);
        }

        /// <summary>
        /// This method executes a test case to verify if the AdjustsSynapsePermanenceBasedOnPreviousActiveCells function
        /// correctly adjusts synapse permanence based on previous active cells. It measures the time taken for the test,
        /// logs the result, and writes the outcome to an Excel file.
        /// </summary>
        public void testcaseAdjustsSynapsePermanenceBasedOnPreviousActiveCells()
        {
            // Initialize an ExperimentResult object to store the results of the test case
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test case
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the function under test and capture the result
            int result1 = adaptSegments.AdjustsSynapsePermanenceBasedOnPreviousActiveCells();

            // Record the end time of the test case
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Assign a unique ID to this test case for identification
            res.ExperimentId = "AdjustsSynapsePermanenceBasedOnPreviousActiveCells";

            // Calculate the duration of the test case execution
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Determine the outcome of the test case based on the result
            if (result1 == 1)
            {
                // If result is 1, the test case is considered passed
                Console.Out.WriteLineAsync("AdjustsSynapsePermanenceBasedOnPreviousActiveCells Test Case Passed");
                res.testcase = "Passed";
                res.Comments = "Test passed successfully.";
            }
            else if (result1 == 0)
            {
                // If result is 0, the test case is considered failed
                Console.Out.WriteLineAsync("AdjustsSynapsePermanenceBasedOnPreviousActiveCells Test Case Failed");
                res.testcase = "Failed";
                res.Comments = "Test failed.";
            }

            // Note that no external input file is needed for this test case
            res.InputFileUrl = "No External Input File needed";

            // Write the result of the test case to an Excel file
            Excel.WriteDataToExcel(res);
        }

        /// <summary>
        /// Executes the test case to verify that synapses are preserved when handling very large negative permanence values.
        /// This test ensures that the system can handle extreme negative permanence values without issues.
        /// The results of the test are logged and written to an Excel file for record-keeping.
        /// </summary>
        public void testcasePreservesSynapses_ForVeryLargeNegativePermanenceValues()
        {
            // Create an instance of ExperimentResult to capture the test details and results.
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test execution in UTC.
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the method to perform the test and capture the result.
            // The method adaptSegments.PreservesSynapses_ForVeryLargeNegativePermanenceValues() should be implemented
            // to execute the actual test logic.
            int result1 = adaptSegments.PreservesSynapses_ForVeryLargeNegativePermanenceValues();

            // Record the timestamp immediately after the test has completed.
            res.Timestamp = DateTime.UtcNow;

            // Record the end time of the test execution in UTC.
            res.EndTimeUtc = DateTime.UtcNow;

            // Assign a unique identifier for this test case to the experiment result.
            res.ExperimentId = "PreservesSynapses_ForVeryLargeNegativePermanenceValues";

            // Calculate the duration of the test execution in seconds.
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Log the test result to the console and update the experiment result status.
            if (result1 == 1)
            {
                Console.Out.WriteLineAsync("PreservesSynapses_ForVeryLargeNegativePermanenceValues Test Case Passed");
                res.testcase = "Passed";
                res.Comments = "Test passed successfully.";
            }
            else if (result1 == 0)
            {
                Console.Out.WriteLineAsync("PreservesSynapses_ForVeryLargeNegativePermanenceValues Test Case Failed");
                res.testcase = "Failed";
                res.Comments = "Test failed.";
            }

            // Set the input file URL, which is not needed for this test case.
            res.InputFileUrl = "No External Input File needed";

            // Write the results of the test to an Excel file for future reference and analysis.
            Excel.WriteDataToExcel(res);
        }

        /// <summary>
        /// Executes a test case to verify that the method `PreservesSynapses_ForZeroPermanenceValues` correctly handles zero permanence values.
        /// This test case measures the time taken to execute the test and records the result in an `ExperimentResult` object.
        /// </summary>
        public void testcasePreservesSynapses_ForZeroPermanenceValues()
        {
            // Create a new instance of ExperimentResult to store test result details
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test case execution
            res.StartTimeUtc = DateTime.UtcNow;

            // Execute the method being tested and capture the result
            int result1 = adaptSegments.PreservesSynapses_ForZeroPermanenceValues();

            // Record the end time of the test case execution
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;

            // Assign a unique identifier to this test case result
            res.ExperimentId = "PreservesSynapses_ForZeroPermanenceValues";

            // Calculate the elapsed time of the test case execution
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Evaluate the result and log the outcome to the console
            if (result1 == 1)
            {
                Console.Out.WriteLineAsync("PreservesSynapses_ForZeroPermanenceValues Test Case Passed");
                res.testcase = "Passed";  // Update the test case result status
                res.Comments = "Test passed successfully.";  // Provide a comment on the test result
            }
            else if (result1 == 0)
            {
                Console.Out.WriteLineAsync("PreservesSynapses_ForZeroPermanenceValues Test Case Failed");
                res.testcase = "Failed";  // Update the test case result status
                res.Comments = "Test failed.";  // Provide a comment on the test result
            }

            // Set the URL for the input file (none required for this test case)
            res.InputFileUrl = "No External Input File needed";

            // Write the test result data to an Excel file for record-keeping
            Excel.WriteDataToExcel(res);
        }



        /// <summary>
        /// Test case to verify if empty segments are handled correctly.
        /// This method invokes the `Verify_Emptysegement` method on the `adaptSegments` object,
        /// checks the result, and logs whether the test case passed or failed. The duration
        /// of the test case execution is also recorded.
        /// </summary>
        public void testcaseVerify_Emptysegement()
        {
            // Create an instance of ExperimentResult to record the test results
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test case
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the method to be tested and store the result
            int result1 = adaptSegments.Verify_Emptysegement();

            // Record the timestamp when the test case was processed
            res.Timestamp = DateTime.UtcNow;

            // Record the end time of the test case
            res.EndTimeUtc = DateTime.UtcNow;

            // Set the unique identifier for this test case
            res.ExperimentId = "Verify_Emptysegement";

            // Calculate the duration of the test case
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Check the result and log whether the test case passed or failed
            if (result1 == 1)
            {
                Console.Out.WriteLineAsync("Verify_Emptysegement Test Case Passed");
                res.testcase = "Passed";
                res.Comments = "Test passed successfully.";
            }
            else if (result1 == 0)
            {
                Console.Out.WriteLineAsync("Verify_Emptysegement Test Case Failed");
                res.testcase = "Failed";
                res.Comments = "Test failed.";
            }

            // Specify that no external input file was needed for this test case
            res.InputFileUrl = "No External Input File needed";

            // Write the results to an Excel file
            Excel.WriteDataToExcel(res);
        }

        /// <summary>
        /// Test case to verify if a segment is correctly killed even if only one synapse is left.
        /// This method calls the `AdaptSegments_UnitTest_KillSegmentEvenIfOnlyoneSynapse_is_left`
        /// method on the `adaptSegments` object, evaluates the result, and logs the outcome of the test case.
        /// The execution time is also recorded.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_KillSegmentEvenIfOnlyoneSynapse_is_left()
        {
            // Create an instance of ExperimentResult to record the test results
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test case
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the method to be tested and store the result
            int result1 = adaptSegments.AdaptSegments_UnitTest_KillSegmentEvenIfOnlyoneSynapse_is_left();

            // Record the timestamp when the test case was processed
            res.Timestamp = DateTime.UtcNow;

            // Record the end time of the test case
            res.EndTimeUtc = DateTime.UtcNow;

            // Set the unique identifier for this test case
            res.ExperimentId = "AdaptSegments_UnitTest_KillSegmentEvenIfOnlyoneSynapse_is_left";

            // Calculate the duration of the test case
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Check the result and log whether the test case passed or failed
            if (result1 == 1)
            {
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_KillSegmentEvenIfOnlyoneSynapse_is_left Test Case Passed");
                res.testcase = "Passed";
                res.Comments = "Test passed successfully.";
            }
            else if (result1 == 0)
            {
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_KillSegmentEvenIfOnlyoneSynapse_is_left Test Case Failed");
                res.testcase = "Failed";
                res.Comments = "Test failed.";
            }

            // Specify that no external input file was needed for this test case
            res.InputFileUrl = "No External Input File needed";

            // Write the results to an Excel file
            Excel.WriteDataToExcel(res);
        }

        /// <summary>
        /// Test case to check if a segment survives under certain conditions.
        /// This method calls the `AdaptSegments_UnitTest_CheckIfSegmentSurvives` method on the
        /// `adaptSegments` object, evaluates the result, and logs the outcome of the test case.
        /// The execution time is also recorded.
        /// </summary>
        public void testcaseAdaptSegments_UnitTest_CheckIfSegmentSurvives()
        {
            // Create an instance of ExperimentResult to record the test results
            ExperimentResult res = new ExperimentResult(null, null);

            // Record the start time of the test case
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the method to be tested and store the result
            int result1 = adaptSegments.AdaptSegments_UnitTest_CheckIfSegmentSurvives();

            // Record the timestamp when the test case was processed
            res.Timestamp = DateTime.UtcNow;

            // Record the end time of the test case
            res.EndTimeUtc = DateTime.UtcNow;

            // Set the unique identifier for this test case
            res.ExperimentId = "AdaptSegments_UnitTest_CheckIfSegmentSurvives";

            // Calculate the duration of the test case
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Check the result and log whether the test case passed or failed
            if (result1 == 1)
            {
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_CheckIfSegmentSurvives Test Case Passed");
                res.testcase = "Passed";
                res.Comments = "Test passed successfully.";
            }
            else if (result1 == 0)
            {
                Console.Out.WriteLineAsync("AdaptSegments_UnitTest_CheckIfSegmentSurvives Test Case Failed");
                res.testcase = "Failed";
                res.Comments = "Test failed.";
            }

            // Specify that no external input file was needed for this test case
            res.InputFileUrl = "No External Input File needed";

            // Write the results to an Excel file
            Excel.WriteDataToExcel(res);
        }


        /// <summary>
        /// Unit test case to verify the bounds of permanence after adaptation.
        /// </summary>
        /// <param name="VerifyPermanence_InputFile">The input file containing test data for verifying permanence bounds.</param>
        public async void testcaseAdaptSegments_UnitTest_VerifyPermanenceBoundsAfterAdaptation(string VerifyPermanence_InputFile)
        {
            // Define the path to the downloaded files directory and the specific JSON file
            string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "DownloadedFiles");
            string jsonFilePath = Path.Combine(outputFolder, VerifyPermanence_InputFile);

            // Create an instance of ExperimentResult to store the result of the test case
            ExperimentResult res = new ExperimentResult(null, null);
            res.StartTimeUtc = DateTime.UtcNow;

            // Call the method to adapt segments and verify permanence bounds
            var (result1, result2) = await adaptSegments.AdaptSegments_UnitTest_VerifyPermanenceBoundsAfterAdaptation(jsonFilePath);

            // Record the end time of the test case
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;
            res.ExperimentId = "AdaptSegments_UnitTest_VerifyPermanenceBoundsAfterAdaptation";

            // Calculate the duration of the test case
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Log the result and set the appropriate status in the result object
            if (result1 == 1)
            {
                await Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifyPermanenceBoundsAfterAdaptation Test Case Passed");
                res.testcase = "Passed";
                res.Comments = result2;
            }
            else if (result1 == 0)
            {
                await Console.Out.WriteLineAsync("AdaptSegments_UnitTest_VerifyPermanenceBoundsAfterAdaptation Test Case Failed");
                res.testcase = "Failed";
                res.Comments = result2;
            }

            // Set the input file URL and write the result to Excel
            res.InputFileUrl = jsonFilePath;
            Excel.WriteDataToExcel(res);
        }

        /// <summary>
        /// Unit test case to decrement permanence if inactive presynaptic cells.
        /// </summary>
        /// <param name="DecrementPermanence_InputFile">The input file containing test cases for decrementing permanence of inactive presynaptic cells.</param>
        public async void testcaseAdaptSegments_UnitTest_DecrementPermanenceIfInactivePresynapticCells(string DecrementPermanence_InputFile)
        {
            // Define the path to the downloaded files directory and the specific JSON file
            string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "DownloadedFiles");
            string jsonFilePath = Path.Combine(outputFolder, DecrementPermanence_InputFile); // Assuming inputFile is the JSON file

            // Initialize instances for adaptation and connections
            var adaptSegments = new AdaptSegments();
            var testcases = new testcases();
            var conn = new Connections();
            Parameters p = Parameters.getAllDefaultParameters();
            p.apply(conn);

            // Create an instance of ExperimentResult to store the result of the test case
            var res = new ExperimentResult(null, null);
            res.StartTimeUtc = DateTime.UtcNow;
            List<TestCase> testCases;

            // Load test cases from the JSON file
            using (var stream = File.OpenRead(jsonFilePath))
            {
                testCases = await JsonSerializer.DeserializeAsync<List<TestCase>>(stream);
            }

            int result = 0;

            // Process each test case
            foreach (var testCase in testCases)
            {
                // Call the method to decrement permanence if inactive presynaptic cells
                result = adaptSegments.AdaptSegments_UnitTest_DecrementPermanenceIfInactivePresynapticCells(testCase.GetCellnumber, testCase.ActiveCellnumber, testCase.InitialPermanence, conn);

                // Log and print the result of each test case
                if (result == 1)
                {
                    await Console.Out.WriteLineAsync("Test case passed");
                }
                else
                {
                    await Console.Out.WriteLineAsync("Failed");
                }
            }

            // Set the test case result based on the final outcome
            if (result == 1)
            {
                res.testcase = "Passed";
                res.Comments = "Test passed successfully.";
            }

            // Record the end time of the test case and calculate the duration
            res.Timestamp = DateTime.UtcNow;
            res.EndTimeUtc = DateTime.UtcNow;
            res.ExperimentId = "AdaptSegments_UnitTest_DecrementPermanenceIfInactivePresynapticCells";
            var elapsedTime = res.EndTimeUtc - res.StartTimeUtc;
            res.DurationSec = (long)elapsedTime.GetValueOrDefault().TotalSeconds;

            // Set the input file URL and write the result to Excel
            res.InputFileUrl = jsonFilePath;
            Excel.WriteDataToExcel(res);
        }

    }
}
