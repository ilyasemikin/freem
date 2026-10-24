namespace Freem.Domain.Entities.Import;

public sealed class ImportParseResult
{
    public IReadOnlyList<ImportTag> Tags { get; }
    public IReadOnlyList<ImportActivity> Activities { get; }
    public IReadOnlyList<ImportRecord> Records { get; }
    public int RowsInFile { get; }
    public IReadOnlyList<ImportParseWarning> Warnings { get; }

    public ImportParseResult(
        IReadOnlyList<ImportTag> tags,
        IReadOnlyList<ImportActivity> activities,
        IReadOnlyList<ImportRecord> records,
        int rowsInFile,
        IReadOnlyList<ImportParseWarning> warnings)
    {
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(records);
        ArgumentNullException.ThrowIfNull(warnings);
        ArgumentOutOfRangeException.ThrowIfLessThan(rowsInFile, records.Count);

        Tags = tags;
        Activities = activities;
        Records = records;
        RowsInFile = rowsInFile;
        Warnings = warnings;
    }
}
