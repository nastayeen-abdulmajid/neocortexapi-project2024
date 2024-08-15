using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MyExperiment
{
    /// <summary>
    /// Represents a test case with specific parameters for a machine learning experiment.
    /// </summary>
    public class TestCase
    {
        /// <summary>
        /// Gets or sets the number of cells in the test case.
        /// </summary>
        public int GetCellnumber { get; set; }

        /// <summary>
        /// Gets or sets the number of active cells in the test case.
        /// </summary>
        public int ActiveCellnumber { get; set; }

        /// <summary>
        /// Gets or sets the initial permanence value used in the test case.
        /// </summary>
        public double InitialPermanence { get; set; }
    }

    /// <summary>
    /// Provides functionality to load test cases from a JSON file.
    /// </summary>
    public class TestDataReader
    {
        /// <summary>
        /// Loads a list of test cases from a JSON file.
        /// </summary>
        /// <param name="filePath">The path to the JSON file containing the test cases.</param>
        /// <returns>A list of <see cref="TestCase"/> objects loaded from the JSON file.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the specified file does not exist.</exception>
        public static List<TestCase> LoadTestCases(string filePath)
        {
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                // Throw an exception if the file is not found
                throw new FileNotFoundException($"The file at {filePath} does not exist.");
            }

            // Read the content of the file into a string
            string json = File.ReadAllText(filePath);

            // Deserialize the JSON string into a list of TestCase objects
            return JsonConvert.DeserializeObject<List<TestCase>>(json);
        }
    }
}
