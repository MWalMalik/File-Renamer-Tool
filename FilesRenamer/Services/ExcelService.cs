using ClosedXML.Excel;
using System.Globalization;

public static class ExcelService
{
    public static List<Studio> GetStudios()
    {
        var studios = new List<Studio>();

        using (var workbook = new XLWorkbook(FilePaths.Studios))
        {
            foreach (var row in workbook.Worksheet(1).RangeUsed().RowsUsed().Skip(1)) // Skip header row with .Skip(1)
            {
                studios.Add(new Studio
                {
                    MainName = row.Cell(1).GetValue<string>(),
                    AltNames = row.Cell(2).GetValue<string>()?.Split(',').Select(name => name.Trim()).ToList() ?? new List<string>(),
                    VR = row.Cell(3).GetValue<string>().Trim().ToUpper() == "Y"
                });
            }
        }

        return studios;
    }

    public static List<string> GetTags()
    {
        var tags = new List<string>();

        using (var workbook = new XLWorkbook(FilePaths.Tags))
        {
            foreach (var row in workbook.Worksheet(1).RangeUsed().RowsUsed().Skip(1)) // Skip header row with .Skip(1)
            {
                string tag = row.Cell(1).GetValue<string>();
                if (!string.IsNullOrWhiteSpace(tag))
                {
                    tags.Add(tag.Trim());
                }
            }
        }

        return tags;
    }

    public static List<Actor> GetActors()
    {
        var actors = new List<Actor>();

        using (var workbook = new XLWorkbook(FilePaths.Actors))
        {
            foreach (var row in workbook.Worksheet(1).RangeUsed().RowsUsed().Skip(1)) // Skip header row with .Skip(1)
            {
                string dobCellValue = row.Cell(3).GetValue<string>();

                actors.Add(new Actor
                {
                    MainName = row.Cell(1).GetValue<string>(),
                    AltNames = row.Cell(2).GetValue<string>()?.Split(',').Select(name => name.Trim()).ToList() ?? new List<string>(),
                    DOB = string.IsNullOrWhiteSpace(dobCellValue) ? null : DateTime.ParseExact(dobCellValue, "yyyy.MM.dd", CultureInfo.InvariantCulture),
                    Male = row.Cell(4).GetValue<string>().Trim().ToUpper() == "Y",
                    Tags = string.IsNullOrWhiteSpace(row.Cell(5).GetValue<string>()) ? "()" : row.Cell(5).GetValue<string>()
                });
            }
        }

        return actors;
    }

    public static List<string> GetFilterdWords()
    {
        var filterWords = new List<string>();

        using (var workbook = new XLWorkbook(FilePaths.Filter))
        {
            foreach (var row in workbook.Worksheet(1).RangeUsed().RowsUsed().Skip(1)) // Skip header row
            {
                string word = row.Cell(1).GetValue<string>();
                if (!string.IsNullOrWhiteSpace(word))
                {
                    filterWords.Add(word.Trim());
                }
            }
        }

        return filterWords;
    }

}