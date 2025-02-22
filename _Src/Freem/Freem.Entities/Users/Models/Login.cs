using Freem.Clones;
using Freem.Validation;
using Freem.Validation.Extensions;

namespace Freem.Entities.Models.Users;

public sealed class Login : 
    IEquatable<Login>,
    ICloneable<Login>
{
    public static Validator<string> Validator { get; }

    private readonly string _value;

    static Login()
    {
        var builder = new ValidatorBuilder<string>();

        builder = builder
            .ValueMust(value => !string.IsNullOrWhiteSpace(value), "Login cannot be null or empty");

        Validator = builder.Build();
    }

    public Login(string value)
    {
        Validator
            .Validate(value)
            .ThrowIfInvalid();
        
        _value = value;
    }

    public bool Equals(Login? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return _value == other._value;
    }
    
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Login other && Equals(other);
    }
    
    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public Login Clone()
    {
        return this;
    }
    
    public static implicit operator Login(string value)
    {
        return new Login(value);
    }

    public static implicit operator string(Login value)
    {
        return value._value;
    }
}