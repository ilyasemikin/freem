namespace Freem.Domain.Entities.Import;

public sealed class ImportParseResult
{
    public IReadOnlyList<ImportTag> Tags { get; }
    public IReadOnlyList<ImportActivity> Activities { get; }
    public IReadOnlyList<ImportRecord> Records { get; }

    public ImportParseResult(
        IReadOnlyList<ImportTag> tags,
        IReadOnlyList<ImportActivity> activities,
        IReadOnlyList<ImportRecord> records)
    {
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(records);

        Tags = tags;
        Activities = activities;
        Records = records;
    }
}
