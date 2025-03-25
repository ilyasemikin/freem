using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public abstract class BaseController : ControllerBase
{
    
}