using AutoMapper;

namespace CloudDrop.Api.Core.Services.Data;
public class BaseService
{
    protected readonly IMapper _mapper;

    public BaseService(IMapper mapper)
    {
        _mapper = mapper;
    }
}
