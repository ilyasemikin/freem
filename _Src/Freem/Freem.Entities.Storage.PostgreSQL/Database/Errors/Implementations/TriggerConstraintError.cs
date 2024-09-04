using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Abstractions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations.Models;

namespace Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;

internal sealed class TriggerConstraintError : IDatabaseError
{
    private static readonly Regex InputRegex = 
        new(@"\[(?<Code>[A-Za-z]+)\]( {(?<Parameters>.+)})?: (?<Message>.+)", RegexOptions.Compiled);
    
    private const string CodeGroupName = "Code";
    private const string ParametersGroupName = "Parameters";
    private const string MessageGroupName = "Message";
    
    private static readonly Regex ParametersRegex = 
        new(@"(?<Name>[^;}]+)=(?<Value>[^;}]+)(;(?<Name>[^;}]+)=(?<Value>[^;}]+))*", RegexOptions.Compiled);
    
    private const string ParameterNameGroupName = "Name";
    private const string ParameterValueGroupName = "Value";
    
    public string Code { get; }
    public string Message { get; }
    public IReadOnlyList<ErrorParameter> Parameters { get; }

    public TriggerConstraintError(string code, string message, IEnumerable<ErrorParameter>? parameters = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        Code = code;
        Message = message;
        Parameters = parameters?.ToArray() ?? [];
    }

    public TriggerConstraintError(string code, string message, params ErrorParameter[] parameters)
        : this(code, message, (IEnumerable<ErrorParameter>)parameters)
    {
    }
    
    public bool Equals(IDatabaseError? other)
    {
        return other is DatabaseForeignKeyConstraintError error && Equals(error);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is TriggerConstraintError other && Equals(other);
    }
    
    private bool Equals(TriggerConstraintError other)
    {
        return Code == other.Code && 
               Message == other.Message &&
               Parameters.SequenceEqual(other.Parameters);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Code, Message, Parameters);
    }

    public override string ToString()
    {
        var parameters = string.Join(";", Parameters);
        return parameters.Length != 0
            ? $"[{Code}] {{{parameters}}}: {Message}"
            : $"[{Code}]: {Message}";
    }

    public static bool TryParse(string input, [NotNullWhen(true)] out TriggerConstraintError? error)
    {
        error = null;
        
        var match = InputRegex.Match(input);
        if (!match.Groups.TryGetValue(CodeGroupName, out var codeGroup) ||
            !match.Groups.TryGetValue(MessageGroupName, out var messageGroup))
            return false;

        IEnumerable<ErrorParameter>? parameters = null;
        if (match.Groups.TryGetValue(ParametersGroupName, out var parametersGroup) &&
            parametersGroup.Captures.Count > 0 &&
            !TryParseParameters(parametersGroup.Value, out parameters))
            return false;

        var code = codeGroup.Value;
        var message = messageGroup.Value;
        error = new TriggerConstraintError(code, message, parameters);
        return true;
    }
    
    private static bool TryParseParameters(
        string input, 
        [NotNullWhen(true)] out IEnumerable<ErrorParameter>? parameters)
    {
        parameters = null;
        
        var match = ParametersRegex.Match(input);
        if (!match.Success)
            return false;

        if (!match.Groups.TryGetValue(ParameterNameGroupName, out var nameGroup) ||
            !match.Groups.TryGetValue(ParameterValueGroupName, out var valueGroup))
            return false;

        var nameCaptures = nameGroup.Captures;
        var valueCaptures = valueGroup.Captures;
        if (nameCaptures.Count != valueCaptures.Count)
            return false;
        
        var count = nameCaptures.Count;
        if (count == 0)
            return false;

        var result = new List<ErrorParameter>();
        for (var i = 0; i < count; i++)
        {
            var parameterName = nameCaptures[i].Value;
            var parameterValue = valueCaptures[i].Value;
            
            var parameter = new ErrorParameter(parameterName, parameterValue);
            result.Add(parameter);
        }

        parameters = result;
        return true;
    }
}