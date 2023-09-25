using CloudDrop.Api.Core.Contracts.Services.Data;
using CloudDrop.Api.Core.Models.Responses;

using Microsoft.AspNetCore.Mvc;

namespace CloudDrop.Api.Controllers;

[Route("v{version:apiVersion}/users/{userId}/recordings")]
public class RecordingController(IFileService fileService, ILogger<RecordingController> logger) : Base.AuthorizeBaseController
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FileResponse>>> GetRecordings(uint userId, CancellationToken cancellation)
    {
        try
        {
            IEnumerable<FileResponse> files = await fileService.GetRecordingsForUser(userId, cancellation);
            return Ok(files);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to get recordings");
            return BadRequest();
        }
    }
}
