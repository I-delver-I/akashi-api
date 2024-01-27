using AkaShi.Core.DTO.LibraryVersion;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace AkaShi.Core.ServiceContracts;

public interface IFirebaseStorageService : IDisposable
{
    Task<string> MakeFilePublicAndGetUrlAsync(string storageFilePath);
    Task<Object> UploadAsync(Stream fileStream, string storageFilePath, string contentType);
    Task<DownloadLibraryVersionDTO> DownloadAsync(string storageFilePath);
    Task RemoveAsync(string storagePath);
    Task<Object> UpdateAsync(Stream newContent, string storageFilePath, string contentType);
}