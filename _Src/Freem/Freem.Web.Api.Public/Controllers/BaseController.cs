using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public abstract class BaseController : ControllerBase
{
    protected static IActionResult CreateResult(int statusCode)
    {
        return new StatusCodeResult(statusCode);
    }
    
    protected static IActionResult CreateResult(object value, int statusCode)
    {
        return new ObjectResult(value)
        {
            StatusCode = statusCode
        };
    }
}