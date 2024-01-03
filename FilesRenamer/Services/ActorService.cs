
using System.Text.RegularExpressions;

public static class ActorService
{
    public static (string formattedActors, string formattedTitle) GetFormattedActors(string fileName, DateTime? date)
    {
        var actorList = ExcelService.GetActors();
        var actorNames = new HashSet<string>();

        foreach (var actor in actorList)
        {
            if (actor.MatchFound(fileName))
            {
                // If match is found, remove the matched text from the file name
                fileName = Regex.Replace(fileName, actor.RegexPattern, "", RegexOptions.IgnoreCase);
                // Then, update actor names
                actorNames.Add(GetFormattedActorName(actor.MainName, actor.DOB, actor.Male));
                // Then, update actor.tags with age
                actor.UpdateTagsWithAge(date);
                // Then, update tags in filename
                fileName += actor.Tags;
            }
        }

        return (string.Join(", ", actorNames), fileName);
    }

    private static string GetFormattedActorName(string mainName, DateTime? dob, bool isMale)
    {
        // Replace spaces in the main name with periods
        string formattedName = mainName.Replace(" ", ".");

        if (!isMale && dob.HasValue)
        {
            // Append the year from the DOB
            formattedName += $".{dob.Value.Year}";

            // If the birth month is August or later, append the first letter of the month
            if (dob.Value.Month >= 8)
            {
                formattedName += $"'{dob.Value.ToString("MMM").Substring(0, 1)}";
            }
        }
        
        return formattedName;
    }
}
