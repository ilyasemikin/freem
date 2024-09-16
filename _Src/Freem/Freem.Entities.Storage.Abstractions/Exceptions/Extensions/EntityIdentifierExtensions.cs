using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Exceptions.Extensions;

internal static class EntityIdentifierExtensions
{
    private const string Separator = ", ";
    private const string QuotationMark = "\"";

    public static string ToListString(this IEnumerable<IEntityIdentifier> ids)
    {
        return '[' + string.Join(Separator, ids.Select(ToQuotedString)) + ']';
    }

    public static string ToQuotedString(this IEntityIdentifier id)
    {
        return id.Value.ToQuotedString();
    }
}
