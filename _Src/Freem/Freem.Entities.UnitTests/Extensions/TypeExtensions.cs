using Freem.Reflection;

namespace Freem.Entities.UnitTests.Extensions;

internal static class TypeExtensions
{
    public static TheoryData<string> GetPublicConstantsStringValues(this Type type)
    {
        var infos = FieldsExtractor.GetPublicConstants(type);

        var result = new TheoryData<string>();
        foreach (var info in infos)
        {
            if (info.GetValue(null) is not string value)
                throw new InvalidOperationException($"Can't get value of constant \"{type.FullName}.{info.Name}\"");
                
            result.Add(value);
        }

        return result;
    }
}