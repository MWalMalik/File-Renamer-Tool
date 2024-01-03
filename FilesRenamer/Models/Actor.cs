using System.Text.RegularExpressions;

public class Actor
{
    public string MainName { get; set; }
    public List<string> AltNames { get; set; }
    public DateTime? DOB { get; set; }
    public bool Male { get; set; }
    public string Tags { get; set; }

    public string RegexPattern
    {
        get
        {
            var patterns = new List<string>
            {
                CreateBracketEnclosedPattern(MainName, DOB),
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

    private string CreateBracketEnclosedPattern(string name, DateTime? dob)
    {
        // For the main name, create a pattern to match it with period found enclosed in brackets.
        var patternWithPeriod = Regex.Escape(name.Replace(" ", "."));
        var dobSuffix = dob.HasValue ?
                        (dob.Value.Month >= 8 ? $".{dob.Value.Year}'{dob.Value.ToString("MMM").Substring(0, 1)}" : $".{dob.Value.Year}") :
                        "";

        return $"(?i){patternWithPeriod}{dobSuffix}"; // Case-insensitive
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

    // Method to update tags with age based on the movie release date string
    public void UpdateTagsWithAge(DateTime? releaseDate)
    {
        if (releaseDate.HasValue && DOB.HasValue && !Male)
        {
            int age = releaseDate.Value.Year - DOB.Value.Year;
            Tags = $"({age}yo, " + Tags.TrimStart('(');
        }
    }
}
