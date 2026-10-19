namespace Freem.Domain.Storage.Exceptions.Extensions;

internal static class StringExtensions
{
    private const string QuotationMark = "\"";

    public static string ToQuotedString(this string value)
    {
        return QuotationMark + value + QuotationMark;
    }
}
