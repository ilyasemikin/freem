namespace Freem.Entities.UseCases.Contracts.Tags.Update;

public enum UpdateTagErrorCode
{
    NothingToUpdate,
    TagNotFound,
    TagNameAlreadyExists,
    UnknownError
}