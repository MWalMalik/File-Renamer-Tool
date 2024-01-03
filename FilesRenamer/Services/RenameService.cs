public static partial class RenameService
{
    public static void RenameFiles(string path)
    {
        var fileList = Directory.GetFiles(path, "*.mp4", SearchOption.TopDirectoryOnly);

        foreach (var file in fileList)
        {
            var title = Path.GetFileNameWithoutExtension(Path.GetFileName(file));

            (var studio, title) = StudioService.GetFormattedStudio(title);
            (var date, title) = DateService.GetFormattedDate(title);
            (var actors, title) = ActorService.GetFormattedActors(title, date);
            (var tags, title) = TagsService.GetFormattedTags(title);

            title = UtilityService.CleanTitle(title);

            var newFileName = "";
            if (!string.IsNullOrEmpty(studio)) { newFileName += $"{studio}"; }
            if (date.HasValue) { newFileName += $" - {date.Value.ToString("yyyy.MM.dd")}"; }
            if (!string.IsNullOrEmpty(actors)) { newFileName += $" - [{actors}]"; }
            if (!string.IsNullOrEmpty(title)) { newFileName += $" - {title}"; };
            if (!string.IsNullOrEmpty(tags)) { newFileName += $" - {tags}"; }
            newFileName += ".mp4";

            // Record log
            LogsService.RecordLog(Path.GetFileName(file), newFileName);

            // Then, replace the file name
            var newFilePath = Path.Combine(Path.GetDirectoryName(file), newFileName);
            // Then, rename the file
            File.Move(file, newFilePath);
        }
    }

    public static void UndoRename(string path)
    {
        // Read the mapping of new file names to original file names from the Excel log
        var mapping = LogsService.ReadLogMapping();

        var fileList = Directory.GetFiles(path, "*.mp4", SearchOption.TopDirectoryOnly);

        foreach (var file in fileList)
        {
            // Extract the current name of the file
            var currentName = Path.GetFileName(file);

            // Check if this name is in the mapping (i.e., if it was renamed previously)
            if (mapping.TryGetValue(currentName, out var originalName))
            {
                // Construct the full path of the file with its original name
                var newFilePath = Path.Combine(Path.GetDirectoryName(file), originalName);
                // Rename the file back to its original name
                File.Move(file, newFilePath);
            }
        }
    }

    public static void Insert(string path)
    {
        var text = Console.ReadLine();

        var fileList = Directory.GetFiles(path, "*.mp4", SearchOption.TopDirectoryOnly);

        foreach (var file in fileList)
        {
            // Compose new name
            var newFileName = text + "." + (Path.GetFileName(file));
            // Record log
            LogsService.RecordLog(Path.GetFileName(file), newFileName);
            // Then, replace the file name
            var newFilePath = Path.Combine(Path.GetDirectoryName(file), newFileName);
            // Then, rename the file
            File.Move(file, newFilePath);
        }
    }
}
