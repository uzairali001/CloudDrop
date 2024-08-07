﻿using CloudDrop.Api.Core.Models.Responses;

namespace CloudDrop.Shared.Models.Responses;
public class AuthenticationDataResponse
{
    public required int UserId { get; set; }
    public required AccessTokenResponse AccessToken { get; set; }
    public required UserResponse User { get; set; }
    public IEnumerable<RoleResponse> Roles { get; set; } = Enumerable.Empty<RoleResponse>();
}
