using CloudDrop.Api.Core.Contracts.Services.Data;
using CloudDrop.Api.Core.Models.Commands;
using CloudDrop.Api.Core.Models.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CloudDrop.Api.Controllers;

[Route("v{version:apiVersion}/users/{userId}/recordings")]
public class RecordingController(IFileService fileService,
    ILogger<RecordingController> logger,
    IConfiguration configuration,
    IWebHostEnvironment environment) : Base.AuthorizeBaseController
{
    /// <summary>
    /// Gets the recordings for the given user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="cancellation">A cancellation token.</param>
    /// <returns>A Enumerable list of FileResponse containing the details of recordings for the given user.</returns>
    /// <response code="200">Returns the list containing all files for specified User ID.</response>
    /// <response code="500">When unexpected error occurred.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FileResponse>>> GetRecordings(uint userId, CancellationToken cancellation)
    {
        try
        {
            IEnumerable<FileResponse> files = await fileService.GetRecordingsForUserAsync(userId, cancellation);
            return Ok(files);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to get recordings");
            return Problem();
        }
    }

    /// <summary>
    /// Gets the recording file for the given user and file ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="id">The ID of the file.</param>
    /// <param name="cancellation">A cancellation token.</param>
    /// <returns>The recording file for the given user and file ID.</returns>
    /// <response code="200">Returns the requested file.</response>
    /// <response code="404">When unable to find file for the given user and file ID.</response>
    /// <response code="500">When unexpected error occurred.</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRecordingFile(uint userId, uint id, CancellationToken cancellation)
    {
        try
        {
            GetFileCommand command = new()
            {
                UserId = userId,
                FileId = id,
            };
            FileResponse? file = await fileService.GetRecordingFileAsync(command, cancellation);
            if (file is null)
            {
                return NotFound();
            }

            string uploadDirectory = configuration.GetValue<string>("UploadPath")
               ?? throw new Exception("UploadPath not found in settings");

            string filePath = Path.Combine(environment.ContentRootPath, uploadDirectory, userId.ToString(), file.Name);
            PhysicalFileResult fileResult = new(filePath, file.MimeType);
            return fileResult;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to get file");
            return Problem();
        }
    }
}
