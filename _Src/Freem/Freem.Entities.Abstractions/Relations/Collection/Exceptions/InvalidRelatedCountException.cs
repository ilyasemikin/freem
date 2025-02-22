namespace Freem.Entities.Abstractions.Relations.Collection.Exceptions;

public sealed class InvalidRelatedCountException : Exception
{
    public int ExpectedMinCount { get; }
    public int ExpectedMaxCount { get; }
    
    public int ActualCount { get; }

    public InvalidRelatedCountException(int expectedMinCount, int expectedMaxCount, int actualCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(expectedMinCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(expectedMaxCount);
        ArgumentOutOfRangeException.ThrowIfNegative(actualCount);
        
        ExpectedMinCount = expectedMinCount;
        ExpectedMaxCount = expectedMaxCount;
        
        ActualCount = actualCount;
    }
}