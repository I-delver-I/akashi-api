using AkaShi.Core.DTO.LibraryVersionDependency;

namespace AkaShi.Core.ServiceContracts;

public interface ILibraryVersionDependencyService
{
    Task<ICollection<LibraryVersionDependencyDTO>> GetLibraryVersionDependenciesAsync();
    Task<LibraryVersionDependencyDTO> GetLibraryVersionDependencyByIdAsync(int id);
    Task<LibraryVersionDependencyDTO> CreateLibraryVersionDependencyAsync
        (NewLibraryVersionDependencyDTO newLibraryVersionDependencyDto);
    Task DeleteLibraryVersionDependencyAsync(int id);
}