using AutoMapper;

using CloudDrop.Api.Core.Entities;
using CloudDrop.Api.Core.Models.Responses;

namespace CloudDrop.Api.Core.MapperProfiles;
public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleEntity, RoleResponse>();
    }
}
