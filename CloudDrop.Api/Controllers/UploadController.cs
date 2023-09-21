using CloudDrop.Api.Controllers.Base;
using CloudDrop.Api.Core.Contracts.Services.Data;
using CloudDrop.Shared.Models.Requests;

using Microsoft.AspNetCore.Mvc;

using System.Net.Http.Headers;

namespace CloudDrop.Api.Controllers;


[Route("v{version:apiVersion}/upload")]
public class UploadController : AuthorizeBaseController
{
    private readonly IUploadSessionService _uploadSessionService;
    private readonly IConfiguration _configuration;

    public UploadController(IUploadSessionService uploadSessionService,
        IConfiguration configuration)
    {
        _uploadSessionService = uploadSessionService;
        _configuration = configuration;
    }


    [Consumes("multipart/form-data")]
    [HttpPut("{sessionId}")]
    public async Task<ActionResult> UploadFileAsChunk(string sessionId, IFormFile chunk, CancellationToken cancellation)
    {
        var contentRange = HttpContext.Request.Headers.ContentRange;
        if (string.IsNullOrEmpty(contentRange))
        {
            return BadRequest("Content-Range header is missing");
        }

        ContentRangeHeaderValue contentRangeValue = ContentRangeHeaderValue.Parse(contentRange!);
        if (!contentRangeValue.HasLength)
        {
            return BadRequest("Content-Range is missing length part");
        }
        if (!contentRangeValue.HasRange)
        {
            return BadRequest("Content-Range is missing range part");
        }

        UpdateUploadSessionRequest request = new()
        {
            Size = contentRangeValue.Length!.Value,
            BytesFrom = contentRangeValue.From!.Value,
            BytesTo = contentRangeValue.To!.Value,
            File = chunk,
            SessionId = sessionId,
        };

        bool isSuccess = await _uploadSessionService.UpdateUploadSessionAsync(request, cancellation);
        if (!isSuccess)
        {
            return BadRequest();
        }

        return request.BytesTo+1 == request.Size
            ? StatusCode(201)
            : Accepted();
    }


    //[HttpPut("{sessionId}")]
    //public async Task<ActionResult> UploadFileAsChunk(string sessionId, [FromForm] IFormFile file, CancellationToken cancellation)
    //{
    //    var request = HttpContext.Request;


    //    MemoryStream memoryStream = new();
    //    file.CopyTo(memoryStream);

    //    string content = Encoding.UTF8.GetString(memoryStream.ToArray());

    //    return Ok(content);
    //}

    /// <summary>
    /// Destroys an existing upload session.
    /// </summary>
    /// <param name="id">The ID of the upload session to be destroyed.</param>
    /// <param name="cancellation">The cancellation token.</param>
    [HttpDelete("{sessionId}")]
    public async Task<ActionResult> DestroySession(string sessionId, CancellationToken cancellation)
    {
        bool isDistroyed = await _uploadSessionService.DestroySessionAsync(sessionId, cancellation);
        return isDistroyed ? Ok() : NotFound();
    }
}
