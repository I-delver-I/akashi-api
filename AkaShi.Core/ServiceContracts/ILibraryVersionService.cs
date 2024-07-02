using AkaShi.Core.DTO.LibraryVersion;
using AkaShi.Core.DTO.LibraryVersionDependency;
using AkaShi.Core.Helpers;
using AkaShi.Core.Helpers.RepositoryParams;

namespace AkaShi.Core.ServiceContracts;

public interface ILibraryVersionService
{
    Task<ICollection<LibraryVersionDependencyDTO>> GetLibraryVersionDependenciesAsync(int libraryVersionId);
    Task<IEnumerable<LibraryVersionDTO>> GetLibraryVersionsByLibraryIdAsync(int libraryId);
    Task<DownloadLibraryVersionDTO> DownloadLibraryVersionAsync(int id, string archiveFormat);
    Task<PagedList<LibraryVersionDTO>> GetLibraryVersionsAsync(LibraryVersionParams libraryVersionParams);
    Task<ICollection<LibraryVersionDTO>> GetLibraryVersionsAsync();
    Task<LibraryVersionDTO> GetLibraryVersionByIdAsync(int id);
    Task<LibraryVersionDTO> CreateLibraryVersionAsync(NewLibraryVersionDTO newLibraryVersionDto);
    Task UpdateLibraryVersionAsync(UpdateLibraryVersionDTO updateLibraryVersionDTO);
    Task IncrementLibraryVersionDownloadsAsync(int libraryVersionId);
    Task DeleteLibraryVersionAsync(int id);
}