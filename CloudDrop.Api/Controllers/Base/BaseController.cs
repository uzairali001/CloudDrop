using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;

using System.Net.Mime;

namespace CloudDrop.Api.Controllers.Base;

[ApiController]
[ApiVersion(1.0)]
[Route("v{version:apiVersion}/[controller]")]
//[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class BaseController : ControllerBase
{

}