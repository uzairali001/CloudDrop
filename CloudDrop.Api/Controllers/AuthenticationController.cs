using AutoMapper;

using CloudDrop.Api.Core.Contracts.Services.General;
using CloudDrop.Shared.Models.Requests;
using CloudDrop.Shared.Models.Responses;

using Microsoft.AspNetCore.Mvc;

namespace CloudDrop.Api.Controllers;

[Route("v{version:apiVersion}/auth")]
public class AuthenticationController : Base.BaseController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IMapper _mapper;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    /// <summary>
    /// Authenticates a user with the specified email address and password.
    /// </summary>
    /// <param name="req">The email address and password of the user to authenticate.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/auth
    ///     {
    ///        "email": "testuser@test.com",
    ///        "password": "testuser"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Whether user is authenticated, status code, message, user object along with access token and refresh token</response>
    [HttpPost("signin")]
    public async Task<ActionResult<AuthenticationResponse>> Authenticate([FromBody] AuthenticationRequest req, CancellationToken cancellation)
    {
        AuthenticationResponse response = await _authenticationService.AuthenticateAsync(req, cancellation);

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
    ///        "password": "testuser"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">When user registered successfully</response>
    /// <response code="400">Unable to register user</response>
    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> Register([FromBody] SaveUserRequest req, CancellationToken cancellation)
    {
        try
        {
            UserResponse user = await _authenticationService.RegisterAsync(req, cancellation);
            return StatusCode(201, user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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
