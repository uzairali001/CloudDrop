using CloudDrop.Api.Controllers.Base;
using CloudDrop.Api.Core.Constants;
using CloudDrop.Api.Core.Contracts.Services.Data;
using CloudDrop.Api.Core.Extensions;
using CloudDrop.Shared.Models.Commands;
using CloudDrop.Shared.Models.Requests;
using CloudDrop.Shared.Models.Responses;

using Microsoft.AspNetCore.Mvc;

namespace CloudDrop.Api.Controllers;

/// <summary>
/// This controller handles the creation, destruction, and retrieval of upload sessions.
/// An upload session is a temporary storage space for uploaded files. It is created when a user starts an upload, and it is destroyed when the upload is complete or the user cancels the upload.
/// </summary>
[Route("v{version:apiVersion}/upload/session")]
public class UploadSessionController : AuthorizeBaseController
{
    private readonly IUploadSessionService _uploadSessionService;
    private readonly ILogger<UploadSessionController> _logger;

    public UploadSessionController(IUploadSessionService uploadSessionService,
        ILogger<UploadSessionController> logger)
    {
        _uploadSessionService = uploadSessionService;
        _logger = logger;
    }


    /// <summary>
    /// Creates a new upload session.
    /// </summary>
    /// <param name="req">The name and description of the file being uploaded and conflict behaviour when file already exist</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>The ID of the newly created upload session.</returns>
    [HttpPost]
    public async Task<ActionResult<UploadSessionResponse>> CreateSession(CreateUploadSessionRequest req, CancellationToken cancellation)
    {
        try
        {
            CreateUploadSessionCommand command = new()
            {
                Name = req.Name,
                ConflictBehavior = req.ConflictBehavior,
                Description = req.Description,
                UserId = User.RequiredFirstValue<uint>(ClaimsConstant.Id),
            };
            var resp = await _uploadSessionService.CreateUploadSessionAsync(command, cancellation);

            return resp is not null
                ? Ok(resp) : BadRequest(resp);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to create Upload Session");
            return BadRequest();
        }
    }

    ///// <summary>
    ///// Destroys an existing upload session.
    ///// </summary>
    ///// <param name="id">The ID of the upload session to be destroyed.</param>
    ///// <param name="cancellation">The cancellation token.</param>
    //[HttpDelete("{id}")]
    //public async Task<ActionResult> DestroySession(string id, CancellationToken cancellation)
    //{
    //    return Ok();
    //}
}
