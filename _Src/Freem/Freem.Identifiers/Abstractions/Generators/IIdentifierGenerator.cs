namespace Freem.Identifiers.Abstractions.Generators;

public interface IIdentifierGenerator
{
    IIdentifier Generate();
}

public interface IIdentifierGenerator<out TIdentifier> : IIdentifierGenerator
    where TIdentifier : IIdentifier
{
    new TIdentifier Generate();
    
    IIdentifier IIdentifierGenerator.Generate() => Generate();
}