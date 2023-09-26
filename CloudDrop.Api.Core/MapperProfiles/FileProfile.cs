using AutoMapper;

using CloudDrop.Api.Core.Entities;
using CloudDrop.Api.Core.Models.Responses;

namespace CloudDrop.Api.Core.MapperProfiles;
public class FileProfile : Profile
{
    public FileProfile()
    {
        CreateMap<FileEntity, FileResponse>();
    }
}
