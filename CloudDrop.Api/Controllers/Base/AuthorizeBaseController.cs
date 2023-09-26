using Microsoft.AspNetCore.Authorization;

namespace CloudDrop.Api.Controllers.Base;

[Authorize]
public class AuthorizeBaseController : BaseController
{

}