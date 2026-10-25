using Utilities.Errors;

namespace Freem.DataTransfer.NativeJson;

public static class NativeJsonErrorCatalog
{
    public const string Scope = "freem.data_transfer.native_json";

    public static class Types
    {
        public static readonly ErrorType InvalidJson = ErrorType.Create(Scope, "invalid_json");
        public static readonly ErrorType EmptyFile = ErrorType.Create(Scope, "empty_file");
        public static readonly ErrorType UnexpectedFormat = ErrorType.Create(Scope, "unexpected_format");
        public static readonly ErrorType UnsupportedVersion = ErrorType.Create(Scope, "unsupported_version");
    }

    public static class Factories
    {
        public static Error CreateInvalidJson(string reason)
        {
            return new Error.Builder()
                .WithType(Types.InvalidJson)
                .WithMessage($"File is not a valid JSON: {reason}");
        }

        public static Error CreateEmptyFile()
        {
            return new Error.Builder()
                .WithType(Types.EmptyFile)
                .WithMessage("File is empty");
        }

        public static Error CreateUnexpectedFormat(string? format)
        {
            return new Error.Builder()
                .WithType(Types.UnexpectedFormat)
                .WithMessage($"Unexpected format \"{format}\", expected \"{NativeJsonSchema.FormatMarker}\"");
        }

        public static Error CreateUnsupportedVersion(int? version)
        {
            return new Error.Builder()
                .WithType(Types.UnsupportedVersion)
                .WithMessage($"Unsupported version \"{version}\", supported version is {NativeJsonSchema.CurrentVersion}");
        }
    }
}
