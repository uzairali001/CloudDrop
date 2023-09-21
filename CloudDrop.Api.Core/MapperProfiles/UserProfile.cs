using AutoMapper;

using CloudDrop.Api.Core.Entities;
using CloudDrop.Shared.Models.Requests;
using CloudDrop.Shared.Models.Responses;

namespace CloudDrop.Api.Core.MapperProfiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserResponse>();
        CreateMap<SaveUserRequest, UserEntity>();
    }
}
