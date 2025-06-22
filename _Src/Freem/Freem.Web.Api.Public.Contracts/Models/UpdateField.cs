namespace Freem.Web.Api.Public.Contracts.Models;

public sealed class UpdateField<TValue>
{
    public TValue Value { get; }
    
    public UpdateField(TValue value)
    {
        Value = value;
    }
}