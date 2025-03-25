using System.Diagnostics.CodeAnalysis;
using System.Net;
using Freem.Enums.Exceptions;

namespace Freem.Web.Api.Public.Client.Models;

public class ClientResult
{
    public bool Success => (int)StatusCode >= 200 && (int)StatusCode <= 299;
    
    public HttpStatusCode StatusCode { get; }

    public ClientResult(HttpStatusCode statusCode)
    {
        InvalidEnumValueException<HttpStatusCode>.ThrowIfValueInvalid(statusCode);
        
        StatusCode = statusCode;
    }
    
    public void EnsureSuccess()
    {
        if (!Success)
            throw new Exception();
    }
}

public sealed class ClientResult<T> : ClientResult
{
    [MemberNotNullWhen(true, nameof(Value))]
    public new bool Success => base.Success;
    
    public T? Value { get; }
    
    public ClientResult(HttpStatusCode statusCode, T? value) 
        : base(statusCode)
    {
        Value = value;
    }

    [MemberNotNull(nameof(Value))]
    public new void EnsureSuccess()
    {
        base.EnsureSuccess();
    }
}