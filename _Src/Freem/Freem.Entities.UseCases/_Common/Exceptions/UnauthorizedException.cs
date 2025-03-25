using Freem.UseCases.Abstractions.Exceptions;

namespace Freem.Entities.UseCases.Exceptions;

public sealed class UnauthorizedException : UseCaseException<UseCaseExecutionContext>
{
    public UnauthorizedException(UseCaseExecutionContext context) 
        : base(context)
    {
    }
}