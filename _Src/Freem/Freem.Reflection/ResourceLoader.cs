using System.Reflection;

namespace Freem.Reflection;

public static class ResourceLoader
{
    public static Stream Load<T>(string name)
    {
        return Load(typeof(T), name);
    }
    
    public static Stream Load(Type assemblyType, string name)
    {
        var assembly = Assembly.GetAssembly(assemblyType);
        if (assembly is null)
            throw new InvalidOperationException($"Assembly by type \"{assemblyType.Name}\" not found");

        return Load(assembly, name);
    }

    public static Stream Load(Assembly assembly, string name)
    {
        var stream = assembly.GetManifestResourceStream(name);
        if (stream is null)
            throw new InvalidOperationException($"Resource \"{name}\" not found");

        return stream;
    }
}
