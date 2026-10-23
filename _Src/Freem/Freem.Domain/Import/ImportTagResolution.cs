using Freem.Domain.Tags;

namespace Freem.Domain.Import;

public sealed class ImportTagResolution : IEquatable<ImportTagResolution>
{
    public static ImportTagResolution CreateNew { get; } = new(null);

    public TagId? ExistingId { get; }

    public bool IsCreateNew => ExistingId is null;

    private ImportTagResolution(TagId? existingId)
    {
        ExistingId = existingId;
    }

    public static ImportTagResolution Existing(TagId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        return new ImportTagResolution(id);
    }

    public bool Equals(ImportTagResolution? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Equals(ExistingId, other.ExistingId);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ImportTagResolution other && Equals(other);
    }

    public override int GetHashCode()
    {
        return ExistingId?.GetHashCode() ?? 0;
    }
}
