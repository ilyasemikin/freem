using Freem.Domain.Activities;

namespace Freem.Domain.Import;

public sealed class ImportActivityResolution : IEquatable<ImportActivityResolution>
{
    public static ImportActivityResolution CreateNew { get; } = new(null);

    public ActivityId? ExistingId { get; }

    public bool IsCreateNew => ExistingId is null;

    private ImportActivityResolution(ActivityId? existingId)
    {
        ExistingId = existingId;
    }

    public static ImportActivityResolution Existing(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        return new ImportActivityResolution(id);
    }

    public bool Equals(ImportActivityResolution? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Equals(ExistingId, other.ExistingId);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ImportActivityResolution other && Equals(other);
    }

    public override int GetHashCode()
    {
        return ExistingId?.GetHashCode() ?? 0;
    }
}
