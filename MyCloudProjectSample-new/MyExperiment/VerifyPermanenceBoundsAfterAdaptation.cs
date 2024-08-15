using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyExperiment
{
    /// <summary>
    /// Represents a test case with specific configuration parameters.
    /// </summary>
    public class TestCase1
    {
        /// <summary>
        /// Gets or sets the number of active cells for the test case.
        /// </summary>
        public int ActiveCellnum { get; set; }

        /// <summary>
        /// Gets or sets the initial permanence value for the test case.
        /// </summary>
        public double InitialPermanence { get; set; }
    }

    /// <summary>
    /// Provides functionality to read test data from a JSON file.
    /// </summary>
    public class TestDataReader1
    {
        /// <summary>
        /// Loads a list of test cases from a JSON file.
        /// </summary>
        /// <param name="filePath">The path to the JSON file containing the test cases.</param>
        /// <returns>A list of <see cref="TestCase1"/> objects representing the test cases.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the file at the specified path does not exist.</exception>
        public static List<TestCase1> LoadTestCases1(string filePath)
        {
            // Check if the file exists at the specified path
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file at {filePath} does not exist.");
            }

            // Read the content of the file as a JSON string
            string json = File.ReadAllText(filePath);

            // Deserialize the JSON string into a list of TestCase1 objects
            return JsonConvert.DeserializeObject<List<TestCase1>>(json);
        }
    }
}
