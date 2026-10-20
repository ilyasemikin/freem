namespace Freem.Api.Public.Contracts.Models;

/// <summary>Wraps a new value for a field in a partial update. An absent (null) wrapper means the field stays unchanged, which lets a present wrapper carry null to clear the field.</summary>
public sealed class UpdateField<TValue>
{
    /// <summary>The value to set.</summary>
    public TValue Value { get; }
    
    public UpdateField(TValue value)
    {
        Value = value;
    }
}