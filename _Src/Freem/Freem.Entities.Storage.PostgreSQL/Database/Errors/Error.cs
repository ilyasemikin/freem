using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Freem.Entities.Storage.PostgreSQL.Database.Errors;

internal sealed class Error : IEquatable<Error>
{
    private const string CodeGroupName = "Code";
    private const string ParametersGroupName = "Parameters";
    private const string MessageGroupName = "Message";

    private static Regex _inputRegex = 
        new(@"\[(?<Code>[A-Za-z]+)\] {(?<Parameters>.+)}: (?<Message>.+)", RegexOptions.Compiled);
    
    private const string ParameterNameGroupName = "Name";
    private const string ParameterValueGroupName = "Value";
    
    private static Regex _parametersRegex = 
        new(@"(?<Name>[^;}]+)=(?<Value>[^;}]+)(;(?<Name>[^;}]+)=(?<Value>[^;}]+))*", RegexOptions.Compiled);

    public string Code { get; }
    public string Message { get; }
    public IReadOnlyList<ErrorParameter> Parameters { get; }

    public Error(string code, string message, IEnumerable<ErrorParameter>? parameters = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        
        Code = code;
        Message = message;
        Parameters = parameters?.ToArray() ?? [];
    }

    public Error(string code, string message, params ErrorParameter[] parameters)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        Code = code;
        Message = message;
        Parameters = parameters.ToArray();
    }

    public static bool TryParse(string input, [NotNullWhen(true)] out Error? error)
    {
        error = null;

        IEnumerable<ErrorParameter>? errorParameters = null;
        var match = _inputRegex.Match(input);
        if (!match.Groups.ContainsKey(CodeGroupName))
            return false;
        if (!match.Groups.ContainsKey(MessageGroupName))
            return false;
        if (match.Groups.ContainsKey(ParametersGroupName) &&
            !TryParseParameters(match.Groups[ParametersGroupName], out errorParameters))
            return false;

        var errorCode = match.Groups[CodeGroupName].Value;
        var errorMessage = match.Groups[MessageGroupName].Value;

        error = new Error(errorCode, errorMessage, errorParameters);
        return true;
        
        static bool TryParseParameters(Group group, [NotNullWhen(true)] out IEnumerable<ErrorParameter>? parameters)
        {
            parameters = null;
        
            var value = group.Value;
            var match = _parametersRegex.Match(value);
        
            var names = match.Groups[ParameterNameGroupName].Captures;
            var values = match.Groups[ParameterValueGroupName].Captures;

            if (names.Count != values.Count)
                return false;
        
            var count = names.Count;
            if (count == 0)
                return false;

            var result = new List<ErrorParameter>();
            for (var i = 0; i < count; i++)
            {
                var parameterName = names[i].Value;
                var parameterValue = values[i].Value;
            
                var parameter = new ErrorParameter(parameterName, parameterValue);
                result.Add(parameter);
            }

            parameters = result;
            return true;
        }
    }

    public bool Equals(Error? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return Code == other.Code && Message == other.Message && Parameters.SequenceEqual(other.Parameters);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Error other && Equals(other);
    }

    public override string ToString()
    {
        var parameters = string.Join(";", Parameters);
        
        return parameters.Length == 0
            ? $"[{Code}]: {Message}"
            : $"[{Code}] {{{parameters}}}: {Message}";
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Code, Message, Parameters);
    }
}