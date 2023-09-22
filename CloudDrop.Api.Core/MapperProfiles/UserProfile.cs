using AutoMapper;

using CloudDrop.Api.Core.Entities;
using CloudDrop.Api.Core.Models.Requests;
using CloudDrop.Shared.Models.Requests;
using CloudDrop.Shared.Models.Responses;

namespace CloudDrop.Api.Core.MapperProfiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserResponse>();
        CreateMap<AddUserRequest, UserEntity>();
        CreateMap<UpdateUserRequest, UserEntity>();
    }
}
