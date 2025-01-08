namespace Freem.Entities.UseCases.Tags.Update.Models;

public enum UpdateTagErrorCode
{
    NothingToUpdate,
    TagNotFound,
    TagNameAlreadyExists,
    UnknownError
}