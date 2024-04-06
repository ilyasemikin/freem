using Freem.Collections.Identifiers.Validation.Abstractions;

namespace Freem.Entities.Helpers;

internal static class IdentifiersCheckerStrategies
{
    public const int CategoryIdsMinCount = 1;
    public const int CategoryIdsMaxCount = 1000;

    public static IIdentifierActionCheckerStrategy CategoryIdsCheckerStrategy = IIdentifierActionCheckerStrategy.CreateRangeCountChecker(
        CategoryIdsMinCount,
        CategoryIdsMaxCount);

    public static IIdentifierActionCheckerStrategy TagIdsCheckerStrategy = IIdentifierActionCheckerStrategy.AllAllowedChecker;
}
