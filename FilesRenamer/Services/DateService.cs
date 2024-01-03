using System.Globalization;
using System.Text.RegularExpressions;

public static class DateService
{
    // Array of date formats to handle different date formats with various separators
    private static readonly string[] DateFormats =
    [
        "yyyy.MM.dd", "yy.MM.dd", "dd.MM.yyyy",
        "yyyy-MM-dd", "yy-MM-dd", "dd-MM-yyyy",
        "yyyy MM dd", "yy MM dd", "dd MM yyyy"
    ];

    public static (DateTime? formattedDate, string formattedTitle) GetFormattedDate(string fileName)
    {
        // Regex pattern to match YYYY.MM.DD, YY.MM.DD, DD.MM.YYYY (with ., -, or space separators)
        string pattern = @"\b(\d{2,4})[-.\s](\d{2})[-.\s](\d{2,4})\b";

        // Match all instances of potential date formats in the file name
        var matches = Regex.Matches(fileName, pattern);

        foreach (Match match in matches)
        {
            // Try to parse the matched string into a DateTime object using the specified date formats
            if (DateTime.TryParseExact(match.Value, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                // If parsing is successful, remove the date string from the file name
                fileName = Regex.Replace(fileName, Regex.Escape(match.Value), "").Trim();

                // Return the formatted date string in "yyyy.MM.dd" format and the updated file name
                return (date, fileName);
            }
        }

        // If no valid date is found, return null for the DateTime and the original file name
        return (null, fileName);
    }

    public static void FormatDate(string path)
    {
        Console.WriteLine("Choose the date format:");
        Console.WriteLine("1. Year Month Day");
        Console.WriteLine("2. Day Month Year");
        Console.WriteLine("3. Month Day Year");

        string choice = Console.ReadLine();
        string pattern = "";

        switch (choice)
        {
            case "1":
                pattern = @"\b(\d{2,4})[-.\s](\d{2})[-.\s](\d{2,4})\b"; // YY MM DD
                break;
            case "2":
                pattern = @"\b(\d{2})[-.\s](\d{2})[-.\s](\d{2,4})\b"; // DD MM YY
                break;
            case "3":
                pattern = @"\b(\d{2})[-.\s](\d{2})[-.\s](\d{2,4})\b"; // MM DD YY
                break;
            default:
                Console.WriteLine("Invalid choice. Defaulting to Year Month Day format.");
                pattern = @"\b(\d{2,4})[-.\s](\d{2})[-.\s](\d{2,4})\b"; // Default to YY MM DD
                break;
        }

        var fileList = Directory.GetFiles(path, "*.mp4", SearchOption.TopDirectoryOnly);

        foreach (var file in fileList)
        {
            var newFileName = (Path.GetFileName(file));

            Match match = Regex.Match(newFileName, pattern);

            if (match.Success && DateTime.TryParseExact(match.Value, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                // Replace Date
                newFileName = newFileName.Replace(match.Value, date.ToString("yyyy.MM.dd"));
                // Record log
                LogsService.RecordLog(Path.GetFileName(file), newFileName);
                // Then, replace the file name
                var newFilePath = Path.Combine(Path.GetDirectoryName(file), newFileName);
                // Then, rename the file
                File.Move(file, newFilePath);
            }
        }
    }
}
