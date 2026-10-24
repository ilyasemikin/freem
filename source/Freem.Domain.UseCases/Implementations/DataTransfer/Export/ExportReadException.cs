using Utilities.Errors;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Export;

public sealed class ExportReadException : Exception
{
    public Error Error { get; }

    public ExportReadException(Error error)
        : base("Failed to read user data for export")
    {
        ArgumentNullException.ThrowIfNull(error);

        Error = error;
    }
}
