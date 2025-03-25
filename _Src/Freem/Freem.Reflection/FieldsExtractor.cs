using System.Reflection;

namespace Freem.Reflection;

public static class FieldsExtractor
{
    public static IEnumerable<FieldInfo> GetPublicConstants(Type type)
    {
        return type
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(info => info is { IsLiteral: true, IsInitOnly: false });
    }
}