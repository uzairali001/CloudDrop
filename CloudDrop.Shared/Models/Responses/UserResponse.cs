﻿namespace CloudDrop.Shared.Models.Responses;
public class UserResponse
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? Username { get; set; }
}