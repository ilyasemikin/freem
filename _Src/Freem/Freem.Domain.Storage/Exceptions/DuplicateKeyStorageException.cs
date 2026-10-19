using Freem.Domain.Storage.Exceptions.Base;

namespace Freem.Domain.Storage.Exceptions;

public sealed class DuplicateKeyStorageException : StorageException
{
    public ErrorCode Code { get; }
    
    public DuplicateKeyStorageException(ErrorCode code) 
        : base(GenerateMessage(code))
    {
        if (!Enum.IsDefined(code))
            throw new ArgumentOutOfRangeException(nameof(code), code, null);
        
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