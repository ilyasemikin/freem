namespace Freem.Storage.Abstractions;

public interface IStorageTransactionExceptionHandler
{
    void Handle(Exception ex);
}