using AkaShi.Core.DTO.LibraryVersionSupportedFramework;

namespace AkaShi.Core.ServiceContracts;

public interface ILibraryVersionSupportedFrameworkService
{
    Task<ICollection<LibraryVersionSupportedFrameworkDTO>> GetLibraryVersionSupportedFrameworksAsync();
    Task<LibraryVersionSupportedFrameworkDTO> GetLibraryVersionSupportedFrameworkByIdAsync(int id);
    Task<LibraryVersionSupportedFrameworkDTO> CreateLibraryVersionSupportedFrameworkAsync
        (NewLibraryVersionSupportedFrameworkDTO dto);
    Task DeleteLibraryVersionSupportedFrameworkAsync(int id);
}