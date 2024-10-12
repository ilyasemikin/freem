namespace Freem.Entities.UseCases.Abstractions.Types;

public sealed class Unit
{
    public static Unit Instance { get; } = new();
    
    private Unit()
    {
    }
}