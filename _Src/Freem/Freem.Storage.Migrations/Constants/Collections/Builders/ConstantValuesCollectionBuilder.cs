namespace Freem.Storage.Migrations.Constants.Collections.Builders;

public sealed class ConstantValuesCollectionBuilder
{
    private readonly Dictionary<string, string> _values;

    public ConstantValuesCollectionBuilder()
    {
        _values = new Dictionary<string, string>();
    }
    
    public ConstantValuesCollectionBuilder WithConstant(string key, string value)
    {
        _values.Add(key, value);
        return this;
    }
    
    public ConstantValuesCollection Build()
    {
        return new ConstantValuesCollection(_values);
    }
}