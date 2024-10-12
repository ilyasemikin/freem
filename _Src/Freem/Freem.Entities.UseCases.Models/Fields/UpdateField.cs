namespace Freem.Entities.UseCases.Models.Fields;

public sealed class UpdateField<TValue>
    where TValue : notnull
{
    public TValue Value { get; }

    public UpdateField(TValue value)
    {
        ArgumentNullException.ThrowIfNull(value);
        
        Value = value;
    }

    public static implicit operator TValue(UpdateField<TValue> updateField)
    {
        return updateField.Value;
    }

    public static implicit operator UpdateField<TValue>(TValue value)
    {
        return new UpdateField<TValue>(value);
    }
}