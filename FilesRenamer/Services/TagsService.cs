using System.Text.RegularExpressions;

public static class TagsService
{
    private static IEnumerable<string> ExtractTagsFromMatch(string match)
    {
        // Removing parentheses and splitting by both comma and dot, then trimming
        var tags = match.Trim('(', ')').Split(new[] { ',', '.' }, StringSplitOptions.RemoveEmptyEntries);

        // Filter out empty or whitespace strings and convert to lowercase
        return tags.Select(tag => tag.Trim())
                   .Where(tag => !string.IsNullOrWhiteSpace(tag))
                   .Select(tag => tag.ToLower());
    }

    private static string FormatTagsForOutput(HashSet<string> tags)
    {
        return tags.Any() ? "(" + string.Join(".", tags) + ")" : string.Empty;
    }

    public static (string formattedTags, string formattedTitle) GetFormattedTags(string fileName)
    {
        var tags = ExcelService.GetTags();

        // Create a regex pattern that matches text in parentheses which contains any of the tags or yo
        string pattern = @"\(([^)]*(" + string.Join("|", tags.Select(Regex.Escape)) + "|" + @"\b\d+yo\b" + @")[^)]*)\)";
        string resolutionPattern = @"\b\d+[kpKP](?=\b|\d)";

        var matches = Regex.Matches(fileName, pattern, RegexOptions.IgnoreCase);
        var resolutionMatches = Regex.Matches(fileName, resolutionPattern, RegexOptions.IgnoreCase);

        var allTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // To handle case-insensitivity
        var resolutionTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // For resolution tags

        foreach (Match match in matches)
        {
            var extractedTags = ExtractTagsFromMatch(match.Value);

            foreach (var tag in extractedTags)
            {
                allTags.Add(tag); // Add to HashSet to keep unique
            }

            fileName = fileName.Replace(match.Value, "");
        }

        foreach (Match match in resolutionMatches)
        {
            resolutionTags.Add(match.Value.ToLower());
            fileName = fileName.Replace(match.Value, "");
        }

        // Combine resolution tags and 'yo' tags at the beginning
        var orderedTags = resolutionTags.Concat(allTags.Where(tag => tag.EndsWith("yo")))
                                        .Concat(allTags.Except(resolutionTags).Where(tag => !tag.EndsWith("yo")))
                                        .ToList();

        return (FormatTagsForOutput(new HashSet<string>(orderedTags)), fileName.Trim());
    }
}
