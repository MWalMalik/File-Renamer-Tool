
using ClosedXML.Excel;

public static class LogsService
{
    public static void FlushLogs()
    {
        using (var workbook = new XLWorkbook(FilePaths.Logs))
        {
            // Access the first worksheet
            var worksheet = workbook.Worksheets.First();

            // Check if there are any rows used beyond the header row
            if (worksheet.LastRowUsed()?.RowNumber() > 1)
            {
                // Get the range that includes all rows except the header
                var range = worksheet.Range(worksheet.Row(2).FirstCell(), worksheet.LastRowUsed().LastCellUsed());
                // Clear the contents of this range
                range.Clear();
            }

            // Save the changes back to the Excel file
            workbook.SaveAs(FilePaths.Logs);
        }
    }

    public static void RecordLog(string originalFileName, string newFileName)
    {
        using (var workbook = new XLWorkbook(FilePaths.Logs))
        {
            var worksheet = workbook.Worksheets.First();

            // Find the next empty row, assuming headers are already present
            int row = worksheet.LastRowUsed()?.RowNumber() + 1 ?? 2;

            // Log the date, original and new filename
            worksheet.Cell(row, 1).Value = DateTime.Now;
            worksheet.Cell(row, 2).Value = originalFileName;
            worksheet.Cell(row, 3).Value = newFileName;

            // Save the workbook
            workbook.SaveAs(FilePaths.Logs);
        }
    }

    public static Dictionary<string, string> ReadLogMapping()
    {
        var map = new Dictionary<string, string>();

        using (var workbook = new XLWorkbook(FilePaths.Logs))
        {
            // Iterate over the rows of the first worksheet, skipping the header row
            foreach (var row in workbook.Worksheet(1).RangeUsed().RowsUsed().Skip(1))
            {
                // Get the original file name from the second column
                var originalName = row.Cell(2).GetValue<string>();

                // Get the new file name from the third column
                var newName = row.Cell(3).GetValue<string>();

                // Add the mapping to the dictionary
                map[newName] = originalName;
            }
        }

        return map;
    }
}
