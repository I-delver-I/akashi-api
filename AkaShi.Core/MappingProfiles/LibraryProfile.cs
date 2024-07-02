using AkaShi.Core.Domain.Entities;
using AkaShi.Core.DTO.Library;
using AkaShi.Core.DTO.LibraryVersion;
using AkaShi.Core.DTO.LibraryVersionDependency;
using AkaShi.Core.DTO.LibraryVersionSupportedFramework;
using AutoMapper;

namespace AkaShi.Core.MappingProfiles;

public class LibraryProfile : Profile
{
    public LibraryProfile()
    {
        CreateMap<Library, LibraryDTO>()
            .ForMember(dest => dest.LogoURL, 
                opt => opt
                    .MapFrom(src => src.Logo.URL))
            .ReverseMap();
        
        CreateMap<NewLibraryDTO, Library>();
        
        CreateMap<LibraryVersionDependency, LibraryVersionDependencyDTO>().ReverseMap();
        
        CreateMap<NewLibraryVersionDependencyDTO, LibraryVersionDependency>();
        
        CreateMap<LibraryVersion, LibraryVersionDTO>()
            .ForMember(dest => dest.FileExtension, 
                opt => opt
                    .MapFrom(src => src.FileExtension.Name))
            .ReverseMap();

        CreateMap<NewLibraryVersionDTO, LibraryVersion>();
        
        CreateMap<NewLibraryVersionSupportedFrameworkDTO, LibraryVersionSupportedFramework>();
        
        CreateMap<LibraryVersionSupportedFramework, LibraryVersionSupportedFrameworkDTO>().ReverseMap();
    }
}