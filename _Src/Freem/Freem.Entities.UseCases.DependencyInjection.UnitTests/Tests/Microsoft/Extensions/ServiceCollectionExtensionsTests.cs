using System.Reflection;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Types;
using Freem.Entities.UseCases.DependencyInjection.Microsoft.Extensions;
using Freem.Reflection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.UseCases.DependencyInjection.UnitTests.Tests.Microsoft.Extensions;

public sealed class ServiceCollectionExtensionsTests
{
    public static TheoryData<Type> UseCasesTypes
    {
        get
        {
            var result = new TheoryData<Type>();
            
            var assembly = Assembly.Load("Freem.Entities.UseCases");
            foreach (var type in assembly.DefinedTypes)
            {
                if (type.TryGetInterface("IUseCase`1", out var @interface))
                    result.Add(@interface);
                else if (type.TryGetInterface("IUseCase`2", out @interface))
                    result.Add(@interface);
            }

            return result;
        }
    }
    
    [Theory]
    [MemberData(nameof(UseCasesTypes))]
    public void AddUseCases_ShouldContainAllUseCases(Type useCaseType)
    {
        var collection = new ServiceCollection();

        collection.AddUseCases();

        var service = collection.FirstOrDefault(e => e.ServiceType == useCaseType);
        
        Assert.NotNull(service);
    }
}