using Freem.Entities.Storage.Abstractions.Exceptions.Base;
using Freem.Enums.Exceptions;

namespace Freem.Entities.Storage.Abstractions.Exceptions;

public sealed class DuplicateKeyStorageException : StorageException
{
    public ErrorCode Code { get; }
    
    public DuplicateKeyStorageException(ErrorCode code) 
        : base(GenerateMessage(code))
    {
        InvalidEnumValueException<ErrorCode>.ThrowIfValueInvalid(code);
        
        Code = code;
    }

    public enum ErrorCode
    {
        DuplicateRunningRecord,
        DuplicateTagName,
        DuplicateUser,
    }

    private static string GenerateMessage(ErrorCode error)
    {
        return error switch
        {
            ErrorCode.DuplicateUser => "Duplicate user",
            _ => "Duplicate error, unknown reason"
        };
    }
}