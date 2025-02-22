using Freem.Clones;
using Freem.Validation;
using Freem.Validation.Extensions;

namespace Freem.Entities.Models.Users;

public sealed class Nickname : 
    IEquatable<Nickname>,
    ICloneable<Nickname>
{
    public static Validator<string> Validator { get; }
    
    private readonly string _value;

    static Nickname()
    {
        var builder = new ValidatorBuilder<string>();

        builder = builder
            .ValueMust(value => !string.IsNullOrWhiteSpace(value), "Nickname cannot be null or empty");

        Validator = builder.Build();
    }
    
    public Nickname(string value)
    {
        Validator
            .Validate(value)
            .ThrowIfInvalid();

        _value = value;
    }

    public bool Equals(Nickname? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Nickname other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static bool operator ==(Nickname left, Nickname right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Nickname left, Nickname right)
    {
        return !(left == right);
    }
    
    public Nickname Clone()
    {
        return this;
    }

    public static implicit operator Nickname(string value)
    {
        return new Nickname(value);
    }
    
    public static implicit operator string(Nickname value)
    {
        return value._value;
    }
}