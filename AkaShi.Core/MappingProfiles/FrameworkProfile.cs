using AkaShi.Core.Domain.Entities;
using AkaShi.Core.DTO.Framework;
using AutoMapper;

namespace AkaShi.Core.MappingProfiles;

public class FrameworkProfile : Profile
{
    public FrameworkProfile()
    {
        CreateMap<Framework, FrameworkDTO>().ReverseMap();
    }
}