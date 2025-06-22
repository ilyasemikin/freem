using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Users.Tokens;

[Route("api/v1/user/cookie-tokens")]
[Tags(ControllerTags.User, ControllerTags.AuthTokens)]
[ProducesResponseType(StatusCodes.Status200OK)]
public sealed class DeleteCookieTokensController : BaseController
{
    [HttpDelete]
    [EndpointSummary("Delete auth cookie tokens")]
    public IActionResult Delete()
    {
        Response.Cookies.Delete(CookieNames.AccessToken);
        Response.Cookies.Delete(CookieNames.RefreshToken);
        
        return Ok();
    }
}