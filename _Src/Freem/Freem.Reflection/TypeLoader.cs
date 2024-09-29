using System.Reflection;
using Freem.Reflection.Extensions;

namespace Freem.Reflection;

public static class TypeLoader
{
    public static IEnumerable<Type> GetImplementations(string assemblyName, string interfaceName)
    {
        var assembly = Assembly.Load(assemblyName);
        if (assembly is null)
            throw new InvalidOperationException($"Unable to load assembly \"{assemblyName}\"");

        foreach (var type in assembly.ExportedTypes)
        {
            if (type.TryGetInterface(interfaceName, out _))
                yield return type;
        }
    }
}