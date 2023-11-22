using AutoMapper;

using CloudDrop.Api.Core.Contracts.Repositories;
using CloudDrop.Api.Core.Contracts.Services.Data;
using CloudDrop.Api.Core.Entities;
using CloudDrop.Api.Core.Models.Requests;
using CloudDrop.Shared.Models.Responses;

namespace CloudDrop.Api.Core.Services.Data;
public class UserService(IMapper mapper, IUserRepository userRepository) : BaseService(mapper), IUserService
{
    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync(IEnumerable<string> roles, uint? userId, CancellationToken cancellation = default)
    {
        IEnumerable<UserEntity> users = await userRepository.GetByRoleAsync(roles, userId, cancellation);
        return _mapper.Map<IEnumerable<UserResponse>>(users);
    }

    public async Task<UserEditResponse?> GetUserByIdAsync(uint id, CancellationToken cancellation = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellation: cancellation);
        return _mapper.Map<UserEditResponse>(user);
    }

    public async Task<bool> AddUserAsync(AddUserRequest request, CancellationToken cancellation = default)
    {
        var entity = _mapper.Map<UserEntity>(request);
        entity.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

        return await userRepository.AddAndSaveAsync(entity, cancellation);
    }

    public async Task<bool> UpdateUserAsync(uint id, UpdateUserRequest request, CancellationToken cancellation = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellation: cancellation);
        if (user is null)
        {
            return false;
        }
        _mapper.Map(request, user);
        if (!string.IsNullOrEmpty(request.Password))
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }

        return await userRepository.UpdateAndSaveAsync(user, cancellation);
    }
}