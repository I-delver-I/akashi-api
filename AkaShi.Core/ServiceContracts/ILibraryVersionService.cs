using AkaShi.Core.DTO.LibraryVersion;

namespace AkaShi.Core.ServiceContracts;

public interface ILibraryVersionService
{
    Task<ICollection<LibraryVersionDTO>> GetLibraryVersionsByLibraryIdAsync(int libraryId);
    Task<DownloadLibraryVersionDTO> DownloadLibraryVersionAsync(int id, string archiveFormat);
    Task<ICollection<LibraryVersionDTO>> GetLibraryVersionsAsync();
    Task<LibraryVersionDTO> GetLibraryVersionByIdAsync(int id);
    Task<LibraryVersionDTO> CreateLibraryVersionAsync(NewLibraryVersionDTO newLibraryVersionDto);
    Task UpdateLibraryVersionAsync(UpdateLibraryVersionDTO updateLibraryVersionDTO);
    Task IncrementLibraryVersionDownloadsAsync(int libraryVersionId);
    Task DeleteLibraryVersionAsync(int id);
}