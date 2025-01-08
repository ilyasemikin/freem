﻿using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Records;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.UseCases.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.Records.Get.Models;

public sealed class GetRecordResponse
{
    [MemberNotNullWhen(true, nameof(Record))]
    public bool Success { get; }
    
    public Record? Record { get; }
    public Error<GetRecordErrorCode>? Error { get; }
    
    private GetRecordResponse(Record? record = null, Error<GetRecordErrorCode>? error = null)
    {
        Success = record is not null;
        Record = record;
        Error = error;
    }

    public static GetRecordResponse CreateSuccess(Record record)
    {
        ArgumentNullException.ThrowIfNull(record);
        
        return new GetRecordResponse(record);
    }

    public static GetRecordResponse CreateFailure(GetRecordErrorCode code, string? message = null)
    {
        var error = new Error<GetRecordErrorCode>(code, message);
        return new GetRecordResponse(error: error);
    }

    internal static GetRecordResponse Create(SearchEntityResult<Record> result)
    {
        return result.Founded
            ? CreateSuccess(result.Entity)
            : CreateFailure(GetRecordErrorCode.RecordNotFound);
    }
}