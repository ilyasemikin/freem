using Utilities.Errors;

namespace Freem.DataTransfer.ZaferErtas.TimeTracker;

public static class TimeTrackerErrorCatalog
{
    public const string Scope = "freem.data_transfer.zafer_ertas.time_tracker";

    public static class Types
    {
        public static readonly ErrorType EmptyFile = ErrorType.Create(Scope, "empty_file");
        public static readonly ErrorType InvalidHeader = ErrorType.Create(Scope, "invalid_header");
    }

    public static class Factories
    {
        public static Error CreateEmptyFile()
        {
            return new Error.Builder()
                .WithType(Types.EmptyFile)
                .WithMessage("File is empty");
        }

        public static Error CreateInvalidHeader()
        {
            return new Error.Builder()
                .WithType(Types.InvalidHeader)
                .WithMessage("File header does not match the Time Tracker export");
        }
    }
}
