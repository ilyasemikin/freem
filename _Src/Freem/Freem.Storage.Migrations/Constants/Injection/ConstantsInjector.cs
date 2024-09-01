using System.Text.RegularExpressions;
using Freem.Storage.Migrations.Constants.Collections;
using Freem.Storage.Migrations.Constants.Injection.Exceptions;

namespace Freem.Storage.Migrations.Constants.Injection;

public sealed class ConstantsInjector
{
    private const string ConstantGroupName = "name";
    
    private static readonly Regex ConstantRegex = new(@"\${(?<name>[A-Za-z].+)}");
    
    private readonly ConstantValuesCollection _values;

    public ConstantsInjector(ConstantValuesCollection values)
    {
        ArgumentNullException.ThrowIfNull(values, nameof(values));
        
        _values = values;
    }

    public string Inject(string input)
    {
        return ConstantRegex.Replace(input, Evaluate);
    }

    private string Evaluate(Match match)
    {
        var group = match.Groups[ConstantGroupName];
        var name = group.Value;
        
        if (!_values.TryGetValue(name, out var value))
            throw new UnknownConstantException(name, match.Index, match.Length);

        return value;
    }
}