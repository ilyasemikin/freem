using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Freem.Reflection.Extensions;

public static class TypeExtensions
{
    public static Type GetRequiredInterface(this Type type, string name)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        
        var @interface = type.GetInterface(name);
        if (@interface is null)
            throw new InvalidOperationException(
                $"The specified type \"{type.Name}\" does not implement the required interface \"{name}\"");

        return @interface;
    }

    public static bool TryGetInterface(this Type type, string name, [NotNullWhen(true)] out Type? @interface)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        
        @interface = type.GetInterface(name);
        return @interface is not null;
    }

    public static MethodInfo GetRequiredMethod(this Type type, string name)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        
        var method = type.GetMethod(name);
        if (method is null)
            throw new InvalidOperationException(
                $"The specified type \"{type.Name}\" does not have the required method \"{name}\"");

        return method;
    }

    public static Type GetRequiredGenericArgument(this Type type, int index)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        
        var arguments = type.GetGenericArguments();
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, arguments.Length);
        
        return arguments[index];
    }
}