namespace Freem.Enums.Exceptions;

public sealed class InvalidEnumValueException<TEnum> : Exception
    where TEnum : struct, Enum
{
    public InvalidEnumValueException(TEnum value)
        : base($"\"{value}\" is not defined in enum \"{nameof(TEnum)}\"")
    {
    }

    public static void ThrowIfValueInvalid(TEnum value)
    {
        if (!Enum.IsDefined(value))
            throw new InvalidEnumValueException<TEnum>(value);
    }
}