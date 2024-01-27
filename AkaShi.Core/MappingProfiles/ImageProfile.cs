using AkaShi.Core.Domain.Entities;
using AkaShi.Core.DTO.Image;
using AutoMapper;

namespace AkaShi.Core.MappingProfiles;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<Image, ImageDTO>()
            .ForMember(dest => dest.FileExtensionName, opt => 
                opt.MapFrom(src => src.FileExtension.Name))
            .ReverseMap();
    }
}