using AutoMapper;

using CloudDrop.Api.Core.Entities;
using CloudDrop.Api.Core.Models.Requests;
using CloudDrop.Shared.Models.Responses;

namespace CloudDrop.Api.Core.MapperProfiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserResponse>()
            .ForMember(dst => dst.UserType, opts => opts.MapFrom(src => src.Type.Name));

        CreateMap<UserEntity, UserEditResponse>();

        CreateMap<AddUserRequest, UserEntity>();
        CreateMap<UpdateUserRequest, UserEntity>();
    }
}
