﻿using CloudDrop.Api.Core.Contracts.Services.Data;
using CloudDrop.Api.Core.Models.Requests;
using CloudDrop.Shared.Models.Responses;

using Microsoft.AspNetCore.Mvc;

namespace CloudDrop.Api.Controllers;

[Route("v{version:apiVersion}/users")]
public class UserController(IUserService userService, ILogger<UserController> logger) : Base.AuthorizeBaseController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers(CancellationToken cancellation)
    {
        IEnumerable<UserResponse> users = await userService.GetAllUsersAsync(cancellation);
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> GetUser(uint id, CancellationToken cancellation)
    {
        try
        {
            UserResponse? user = await userService.GetUserByIdAsync(id, cancellation);
            return user is null ? NotFound() : Ok(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to get user");
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<ActionResult> AddUser(AddUserRequest request, CancellationToken cancellation)
    {
        try
        {
            bool isAdded = await userService.AddUserAsync(request, cancellation);
            return isAdded ? StatusCode(201) : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to add user");
            return BadRequest();
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(uint id, UpdateUserRequest request, CancellationToken cancellation)
    {
        try
        {
            bool isUpdated = await userService.UpdateUserAsync(id, request, cancellation);
            return isUpdated ? NoContent() : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to update user");
            return BadRequest();
        }
    }
}
