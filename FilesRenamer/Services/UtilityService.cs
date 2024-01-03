using System.Globalization;
using System.Text.RegularExpressions;

public static class UtilityService
{
    public static string CleanTitle(string input)
    {
        string[] filterWords = ExcelService.GetFilterdWords().ToArray();

        // Convert all symbols except single quotes to periods
        input = Regex.Replace(input, "[^a-zA-Z0-9']", ".");

        // Replace multiple consecutive periods with a single period
        input = Regex.Replace(input, @"[.]+", ".");

        // Remove periods from the start and end of the string
        input = input.Trim('.');

        // Split the string into words using periods
        var words = input.Split('.');

        // Filter out the specified keywords regardless of case and capitalize each word
        words = words.Where(word => !filterWords.Any(filter => filter.Equals(word, StringComparison.OrdinalIgnoreCase))
                                                               && !Regex.IsMatch(word, @"^\d+yo$", RegexOptions.IgnoreCase))
                     .Select(word => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word.ToLower()))
                     .ToArray();

        // Join the words back into a string with periods
        return String.Join(".", words);
    }
}
