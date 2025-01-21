using System.Diagnostics.CodeAnalysis;

namespace Freem.Results;

public class Result<T>
{
    [MemberNotNullWhen(true, nameof(Value))]
    public bool Success { get; }
    
    public T? Value { get; }

    private Result(bool success, T? value = default)
    {
        Success = success;
        Value = value;
    }

    public static Result<T> CreateSuccess(T value)
    {
        ArgumentNullException.ThrowIfNull(value);
        
        return new Result<T>(true, value);
    }

    public static Result<T> CreateFailure()
    {
        return new Result<T>(false);
    }
}
