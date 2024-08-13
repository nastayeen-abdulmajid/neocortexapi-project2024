using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCloudProject.Common;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExperiment
{
    public class Excel
    {
        /// <summary>
        /// Writes the provided experiment result to an Excel file.
        /// </summary>
        /// <param name="result">The experiment result to write to the Excel file.</param>
        public void WriteDataToExcel(ExperimentResult result)
        {
            // Ensure the Excel package is set to non-commercial license
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Define the Excel file name and path
            string excelName = "table_Result.xlsx";
            string excelFilePath = Path.Combine(Directory.GetCurrentDirectory(), excelName);

            try
            {
                using (var package = new ExcelPackage())
                {
                    // Load the existing Excel file if it exists
                    if (File.Exists(excelFilePath))
                    {
                        var existingFile = new FileInfo(excelFilePath);
                        using (var stream = existingFile.Open(FileMode.Open, FileAccess.ReadWrite))
                        {
                            package.Load(stream);
                        }
                    }

                    // Add or get the worksheet named "Experiment Result"
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Experiment Result");
                    if (worksheet == null)
                    {
                        worksheet = package.Workbook.Worksheets.Add("Experiment Result");

                        // Set headers for the worksheet
                        worksheet.Cells[1, 1].Value = "Timestamp";
                        worksheet.Cells[1, 2].Value = "EndTimeUtc";
                        worksheet.Cells[1, 3].Value = "ExperimentId";
                        worksheet.Cells[1, 4].Value = "DurationSec";
                        worksheet.Cells[1, 5].Value = "InputFileUrl";
                        worksheet.Cells[1, 6].Value = "TestCase";
                        worksheet.Cells[1, 7].Value = "Comments";
                    }

                    // Find the next row to append data
                    int nextRow = worksheet.Dimension?.Rows + 1 ?? 2;

                    // Populate the data row with experiment result values
                    worksheet.Cells[nextRow, 1].Value = result.Timestamp.ToString();
                    worksheet.Cells[nextRow, 2].Value = result.EndTimeUtc.ToString();
                    worksheet.Cells[nextRow, 3].Value = result.ExperimentId;
                    worksheet.Cells[nextRow, 4].Value = result.DurationSec;
                    worksheet.Cells[nextRow, 5].Value = result.InputFileUrl;
                    worksheet.Cells[nextRow, 6].Value = result.testcase;
                    worksheet.Cells[nextRow, 7].Value = result.Comments;

                    // Auto-fit columns for better readability
                    worksheet.Cells.AutoFitColumns();

                    // Save the changes to the Excel file
                    package.SaveAs(new FileInfo(excelFilePath));
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.Error.WriteLine($"Error creating Excel file: {ex.Message}");
            }
        }
    }
}
