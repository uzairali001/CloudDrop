using AutoMapper;

using CloudDrop.Api.Core.Constants;
using CloudDrop.Api.Core.Contracts.Services.General;
using CloudDrop.Api.Core.Extensions;
using CloudDrop.Api.Core.Models.Requests;
using CloudDrop.Shared.Models.Requests;
using CloudDrop.Shared.Models.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrop.Api.Controllers;

[Route("v{version:apiVersion}/auth")]
public class AuthenticationController(
    IAuthenticationService authenticationService) : Base.BaseController
{
    /// <summary>
    /// Authenticates a user with the specified email address and password.
    /// </summary>
    /// <param name="req">The email address and password of the user to authenticate.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/auth
    ///     {
    ///        "username": "testuser@test.com",
    ///        "password": "testuser"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Whether user is authenticated, status code, message, user object along with access token and refresh token</response>
    [HttpPost("signin")]
    public async Task<ActionResult<AuthenticationResponse>> Authenticate(AuthenticationRequest req, CancellationToken cancellation)
    {
        AuthenticationResponse response = await authenticationService.AuthenticateAsync(req, cancellation);

        if (response.IsAuthenticated is false)
        {
            return Unauthorized(response);
        }

        return Ok(response);
    }


    /// <summary>
    /// Registers a new user with the given username and password.
    /// </summary>
    /// <param name="req">The email address, password, level id and manager Id of the user to register.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/auth/register
    ///     {
    ///        "firstName": "Test",
    ///        "lastName": "User",
    ///        "username": "testuser",
    ///        "email": "testuser@test.com",
    ///        "password": "testuser",
    ///        "isActive": true
    ///     }
    ///
    /// </remarks>
    /// <response code="201">When user registered successfully</response>
    /// <response code="400">Unable to register user</response>
    [HttpPost("register")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<UserResponse>> Register([FromBody] AddUserRequest req, CancellationToken cancellation)
    {
        try
        {
            UserResponse user = await authenticationService.RegisterAsync(req, cancellation);
            return StatusCode(201, user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("session")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<AuthenticationDataResponse>> GetUserSession(CancellationToken cancellation)
    {
        uint userId = User.FindFirstValue<uint>(ClaimsConstant.Id);
        AuthenticationDataResponse? response = await authenticationService.GetByUserIdAsync(userId, cancellation);

        return response is null
            ? NotFound()
            : Ok(response);
    }

    ///// <summary>
    ///// Get Refresh token
    ///// </summary>
    ///// <response code="200">Successfully got refresh token</response>
    //[HttpGet("refresh-token")]
    //public async Task<ActionResult> GetRefreshToken([FromServices] ITokenGeneratorService tokenGeneratorService, CancellationToken cancellation)
    //{
    //    var refreshToken = new
    //    {
    //        Token = tokenGeneratorService.GenerateToken(256),
    //        Expires = DateTime.UtcNow.AddDays(7),
    //        Created = DateTime.UtcNow
    //    };

    //    return Ok(refreshToken);
    //}
}
