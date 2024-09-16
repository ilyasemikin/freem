namespace Freem.Entities.Abstractions.Relations.Collection.Exceptions;

public sealed class InvalidRelatedEntitiesCountException : Exception
{
    public int ExpectedMinCount { get; }
    public int ExpectedMaxCount { get; }
    
    public int ActualCount { get; }

    public InvalidRelatedEntitiesCountException(int expectedMinCount, int expectedMaxCount, int actualCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(expectedMinCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(expectedMaxCount);
        ArgumentOutOfRangeException.ThrowIfNegative(actualCount);
        
        ExpectedMinCount = expectedMinCount;
        ExpectedMaxCount = expectedMaxCount;
        
        ActualCount = actualCount;
    }
}