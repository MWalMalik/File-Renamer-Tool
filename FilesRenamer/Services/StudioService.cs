using System.Text.RegularExpressions;

public static class StudioService
{
    public static (string formattedStudio, string formattedTitle) GetFormattedStudio(string fileName)
    {
        var studioList = ExcelService.GetStudios();
        var formattedStudio = "";

        foreach (var studio in studioList)
        {
            if (studio.MatchFound(fileName))
            {
                // If match is found, remove the matched text from the file name
                fileName = Regex.Replace(fileName, studio.RegexPattern, "", RegexOptions.IgnoreCase);
                // Then, remove the [VR] text from the file name if it already exsits as it would be added later
                fileName = fileName.Replace("[VR]", "");
                // Then, compose the new studio name
                formattedStudio = studio.VR ? "[VR] " : "";
                formattedStudio += $"[{studio.MainName.Replace(" ", "")}]";
                break;
            }
        }

        return (formattedStudio, fileName);
    }
}