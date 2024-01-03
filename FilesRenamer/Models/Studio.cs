using System.Text.RegularExpressions;

public class Studio
{
    public string MainName { get; set; }
    public List<string> AltNames { get; set; }
    public bool VR { get; set; }

    public string RegexPattern
    {
        get
        {
            var patterns = new List<string>
            {
                // Add pattern for the main name with bracket-enclosed format.
                CreateBracketEnclosedPattern(MainName),
                // Add general patterns for main and alternate names.
                CreateGeneralRegexPattern(MainName)
            };

            if (AltNames != null)
            {
                patterns.AddRange(AltNames.Where(name => !string.IsNullOrWhiteSpace(name))
                                          .Select(CreateGeneralRegexPattern));
            }

            return string.Join("|", patterns);
        }
    }

    private string CreateBracketEnclosedPattern(string name)
    {
        // For the main name, create a pattern to match it without spaces and enclosed in brackets.
        var patternWithoutSpaces = Regex.Escape(name.Replace(" ", ""));
        return $"(?i)\\[{patternWithoutSpaces}\\]"; // Pattern for [NameWithoutSpaces], case-insensitive
    }

    private string CreateGeneralRegexPattern(string name)
    {
        // Create a pattern that matches the name with '.', no space, a space, or a hyphen.
        return $"(?i){Regex.Escape(name).Replace(@"\ ", @"[.\s\-]*")}"; // Case-insensitive general pattern
    }

    public bool MatchFound(string input)
    {
        return Regex.IsMatch(input, RegexPattern);
    }
}
