namespace Freem.UseCases.Abstractions;

public interface IUseCaseResolver
{
    object Resolve(Type type);
}