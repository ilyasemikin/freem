namespace Freem.UseCases.Abstractions.Exceptions;

public abstract class UseCaseException<TContext> : Exception
{
    public TContext Context { get; }

    protected UseCaseException(TContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        Context = context;
    }
}