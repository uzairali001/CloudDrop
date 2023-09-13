using CloudDrop.Api.Core.Contracts.Services.Data;
using CloudDrop.Api.Core.Models.Requests;
using CloudDrop.Api.Core.Services.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

using System;
using System.Net.Http.Headers;
using System.Text;

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
            ReceivedBytes = contentRangeValue.To!.Value,
            File = chunk,
            SessionId = sessionId,
        };

        bool isSuccess = await _uploadSessionService.UpdateUploadSessionAsync(request, cancellation);
        
        return isSuccess
            ? Ok()
            : BadRequest();
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
