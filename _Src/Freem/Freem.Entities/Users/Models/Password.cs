using System.Text;
using Freem.Clones;
using Freem.Validation;
using Freem.Validation.Extensions;

namespace Freem.Entities.Users.Models;

public sealed class Password :
    IEquatable<Password>,
    ICloneable<Password>
{
    public static Validator<string> Validator { get; }
    
    private readonly string _value;
    
    static Password()
    {
        var builder = new ValidatorBuilder<string>();

        builder
            .ValueMust(value => !string.IsNullOrWhiteSpace(value), "Password cannot be null or empty.");
        
        Validator = builder.Build();
    }
    
    public Password(string value)
    {
        Validator
            .Validate(value)
            .ThrowIfInvalid();
        
        _value = value;
    }

    public byte[] AsBytes()
    {
        return Encoding.UTF8.GetBytes(_value);
    }

    public bool Equals(Password? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Password other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static bool operator ==(Password left, Password right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Password left, Password right)
    {
        return !(left == right);
    }

    public Password Clone()
    {
        return this;
    }

    public static implicit operator Password(string value)
    {
        return new Password(value);
    }

    public static implicit operator string(Password value)
    {
        return value._value;
    }
}