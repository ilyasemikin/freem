using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Freem.Collections.Extensions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Abstractions;

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
    public ParameterCollection Parameters { get; }

    public TriggerConstraintError(string code, string message, ParameterCollection? parameters = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        Code = code;
        Message = message;
        Parameters = parameters ?? new ParameterCollection([]);
    }

    public TriggerConstraintError(string code, string message, params Parameter[] parameters)
        : this(code, message, new ParameterCollection(parameters))
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
               Parameters.Equals(other.Parameters);
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

        ParameterCollection? parameters = null;
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
        [NotNullWhen(true)] out ParameterCollection? parameters)
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

        var result = new List<Parameter>();
        for (var i = 0; i < count; i++)
        {
            var parameterName = nameCaptures[i].Value;
            var parameterValue = valueCaptures[i].Value;
            
            var parameter = new Parameter(parameterName, parameterValue);
            result.Add(parameter);
        }

        parameters = new ParameterCollection(result);
        return true;
    }

    public sealed class ParameterCollection : IEnumerable<Parameter>, IEquatable<ParameterCollection>
    {
        private readonly IReadOnlyDictionary<string, Parameter> _parameters;

        public Parameter this[string name] => _parameters[name];

        public ParameterCollection(IEnumerable<Parameter> parameters)
        {
            ArgumentNullException.ThrowIfNull(parameters);
            
            _parameters = parameters.ToDictionary(p => p.Name);
        }

        public ParameterCollection(params Parameter[] parameters)
        {
            ArgumentNullException.ThrowIfNull(parameters);
            
            _parameters = parameters.ToDictionary(p => p.Name);
        }

        public bool Equals(ParameterCollection? other)
        {
            if (other is null) 
                return false;
            if (ReferenceEquals(this, other)) 
                return true;
            return _parameters.Values.UnorderedEquals(other._parameters.Values);
        }

        public IEnumerator<Parameter> GetEnumerator()
        {
            return _parameters.Values.GetEnumerator();
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is ParameterCollection other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _parameters.GetHashCode();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    public sealed class Parameter : IEquatable<Parameter>
    {
        public string Name { get; }
        public string Value { get; }

        public Parameter(string name, string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            
            Name = name;
            Value = value;
        }

        public bool Equals(Parameter? other)
        {
            if (other is null) 
                return false;
            if (ReferenceEquals(this, other)) 
                return true;
            return Name == other.Name && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is Parameter other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Value);
        }

        public override string ToString()
        {
            return Name + "=" + Value;
        }
    }
}