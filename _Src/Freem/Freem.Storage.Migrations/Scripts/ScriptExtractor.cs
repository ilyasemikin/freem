using Freem.Storage.Migrations.Constants.Injection;
using Freem.Storage.Migrations.Scripts.Models;
using Freem.TextProcessing;

namespace Freem.Storage.Migrations.Scripts;

public sealed class ScriptExtractor
{
    private const string BeginDeclarationLexeme = "Begin declaration";
    private const string EndDeclarationLexeme = "End declaration";
    
    private const string BeginDroppingLexeme = "Begin dropping";
    private const string EndDroppingLexeme = "End dropping";
    
    private readonly ConstantsInjector _constantsInjector;

    public ScriptExtractor(ConstantsInjector constantsInjector)
    {
        ArgumentNullException.ThrowIfNull(constantsInjector);
        
        _constantsInjector = constantsInjector;
    }

    public string Extract(string input, ScriptPart part)
    {
        ArgumentException.ThrowIfNullOrEmpty(input);

        input = _constantsInjector.Inject(input);

        var (start, end) = part switch
        {
            ScriptPart.Declarations => (BeginDeclarationLexeme, EndDeclarationLexeme),
            ScriptPart.Droppings => (BeginDroppingLexeme, EndDroppingLexeme),
            _ => throw new ArgumentOutOfRangeException()
        };

        return LinesExtractor.GetLinesBetween(input, start, end);
    }
}