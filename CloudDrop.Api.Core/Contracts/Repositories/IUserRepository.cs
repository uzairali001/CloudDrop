﻿using CloudDrop.Api.Core.Entities;

namespace CloudDrop.Api.Core.Contracts.Repositories;
public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<bool> AddUserAsync(UserEntity user, CancellationToken cancellation = default);
    Task<UserEntity?> GetByUsernameOrEmailAsync(string username, CancellationToken cancellation = default);
}