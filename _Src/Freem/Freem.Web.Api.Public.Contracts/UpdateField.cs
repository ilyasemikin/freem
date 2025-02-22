namespace Freem.Web.Api.Public.Contracts;

public sealed class UpdateField<TValue>
{
    public bool HasValue { get; }
    public TValue? Value { get; }

    public UpdateField()
    {
        HasValue = false;
        Value = default;
    }

    public UpdateField(TValue value)
    {
        HasValue = true;
        Value = value;
    }
}