using AkaShi.Core.DTO.Library;
using AkaShi.Core.Helpers;
using AkaShi.Core.Helpers.RepositoryParams;

namespace AkaShi.Core.ServiceContracts;

public interface ILibraryService
{
    Task<PagedList<LibraryDTO>> GetLibrariesAsync(LibraryParams libraryParams);
    Task<LibraryDTO> GetLibraryByIdAsync(int id);
    Task<LibraryDTO> CreateLibraryAsync(NewLibraryDTO newLibraryDto);
    Task UpdateLibraryAsync(UpdateLibraryDTO updateLibraryDto);
    Task<LibraryDTO> GetLibraryByNameAsync(string name);
    Task DeleteLibraryAsync(int id);
}