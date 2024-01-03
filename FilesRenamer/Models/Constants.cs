public static class FilePaths
{
    // Base path for the Data directory
    private static readonly string DataDirectoryPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data"));

    // Full paths for each Excel file
    public static readonly string Studios = Path.Combine(DataDirectoryPath, "studios.xlsx");
    public static readonly string Actors = Path.Combine(DataDirectoryPath, "actors.xlsx");
    public static readonly string Logs = Path.Combine(DataDirectoryPath, "logs.xlsx");
    public static readonly string Tags = Path.Combine(DataDirectoryPath, "tags.xlsx");
    public static readonly string Filter = Path.Combine(DataDirectoryPath, "filter.xlsx");
}
