﻿namespace CloudDrop.Api.Core.Models.Responses;
public class AccessTokenResponse
{
    public required string Token { get; set; }
    public required DateTime ExpiryDate { get; set; }
}
