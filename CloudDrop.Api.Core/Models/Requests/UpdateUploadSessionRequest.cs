﻿using Microsoft.AspNetCore.Http;

namespace CloudDrop.Api.Core.Models.Requests;
public class UpdateUploadSessionRequest
{
    public required string SessionId { get; set; }
    public long Size { get; set; }
    public long ReceivedBytes { get; set; }
    public required IFormFile File { get; set; }
}
