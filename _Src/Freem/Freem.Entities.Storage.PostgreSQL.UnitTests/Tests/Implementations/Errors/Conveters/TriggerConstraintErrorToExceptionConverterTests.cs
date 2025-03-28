﻿using System.Reflection;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Converters;
using Freem.Entities.Storage.PostgreSQL.UnitTests.Mocks;
using Freem.Exceptions;

namespace Freem.Entities.Storage.PostgreSQL.UnitTests.Tests.Implementations.Errors.Conveters;

public sealed class TriggerConstraintErrorToExceptionConverterTests
{
    public static TheoryData<string> TriggerErrorCodeCases
    {
        get
        {
            var properties = typeof(TriggerErrorCodes).GetFields(BindingFlags.Public | BindingFlags.Static);
            var values = properties.Select(p => (string)p.GetValue(null)!);

            var data = new TheoryData<string>();
            foreach (var value in values)
                data.Add(value);

            return data;
        }
    }
    
    [Theory]
    [MemberData(nameof(TriggerErrorCodeCases))]
    public void Convert_ShouldNotThrowUnknownConstantException_WhenPassAnyOfTriggerErrorCode(string code)
    {
        var context = new DatabaseContextWriteContext(new SampleIdentifier());
        var error = new TriggerConstraintError(code, "message");
        var converter = new TriggerConstraintErrorToExceptionConverter(new EntityIdentifierFactory());
        
        var exception = Record.Exception(() => converter.Convert(context, error));
        
        Assert.IsNotType<UnknownConstantException>(exception);
    }
}