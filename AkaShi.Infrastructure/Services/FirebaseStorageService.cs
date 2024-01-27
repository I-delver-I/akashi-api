using AkaShi.Core.DTO.LibraryVersion;
using AkaShi.Core.Exceptions;
using AkaShi.Core.ServiceContracts;
using FirebaseAdmin;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Serilog;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace AkaShi.Infrastructure.Services;

public class FirebaseStorageService : IFirebaseStorageService
{
    private const string BucketName = "akashi-a5fff.appspot.com";
    private FirebaseApp _firebaseApp;
    private readonly StorageClient _storageClient;

    public FirebaseStorageService()
    {
        var serviceAccountKeyPath = $"{AppContext.BaseDirectory}/service-account-key.json";
        var credential = GoogleCredential.FromFile(serviceAccountKeyPath);
        
        _storageClient = StorageClient.Create(credential);
        _firebaseApp = FirebaseApp.Create(new AppOptions
        {
            Credential = credential
        });
    }

    public Task<Object> UpdateAsync(Stream newContent, string storageFilePath, string contentType)
    {
        try
        {
            return UploadAsync(newContent, storageFilePath, contentType);
        }
        catch (FirebaseStorageUploadException ex)
        {
            Log.Error("Firebase upload error occurred: {ExMessage}", 
                ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            Log.Error("An error occurred while updating the file: {ExMessage}", 
                ex.Message);
            throw;
        }
    }

    public async Task RemoveAsync(string storagePath)
    {
        if (string.IsNullOrEmpty(storagePath))
        {
            throw new ArgumentException("Storage object path cannot be null or empty.");
        }
        
        try
        {
            if (storagePath.EndsWith("/"))
            {
                var objects = _storageClient.ListObjects(BucketName, storagePath);
                foreach (var obj in objects)
                {
                    await _storageClient.DeleteObjectAsync(BucketName, obj.Name);
                    Log.Information("Deleted object {ObjectName} in folder {FolderPath}", 
                        obj.Name, storagePath);
                }
            }
            else
            {
                await _storageClient.DeleteObjectAsync(BucketName, storagePath);
                Log.Information("Deleted file {FilePath}", storagePath);
            }
        }
        catch (GoogleApiException ex) when (ex.Error.Code == 404)
        {
            Log.Information("Attempted to remove a non-existent file: {FilePath}", 
                storagePath);
        }
        catch (GoogleApiException ex)
        {
            Log.Error("Google API error occurred while removing file: {ExMessage}", 
                ex.Message);
            throw new FirebaseStorageRemoveException(ex);
        }
        catch (Exception e)
        {
            Log.Error("An error occurred while removing file: {ExMessage}", e.Message);
            throw;
        }
    }
    
    public Task<Object> UploadAsync(Stream fileStream, string storageFilePath, string contentType)
    {
        if (string.IsNullOrEmpty(storageFilePath))
        {
            throw new ArgumentException("Storage file path cannot be null or empty.");
        }
        
        if (string.IsNullOrEmpty(contentType))
        {
            throw new ArgumentException("Content type cannot be null or empty.");
        }
        
        try
        {
            return Task.FromResult(_storageClient.UploadObject(BucketName, storageFilePath, 
                contentType, fileStream));
        }
        catch (GoogleApiException ex)
        {
            Log.Error("Google API error occurred: {ExMessage}", ex.Message);
            throw new FirebaseStorageUploadException(ex);
        }
        catch (Exception ex)
        {
            Log.Error("An error occurred while uploading file: {ExMessage}", ex.Message);
            throw; // Rethrow the exception to be handled further up the call stack
        }
    }
    
    public async Task<string> MakeFilePublicAndGetUrlAsync(string storageFilePath)
    {
        if (string.IsNullOrEmpty(storageFilePath))
        {
            throw new ArgumentException("Storage file path cannot be null or empty.");
        }
        
        try
        {
            var storageObject = await _storageClient.GetObjectAsync(BucketName, storageFilePath);
            if (storageObject == null)
            {
                throw new FileNotFoundException
                    ($"The file at path '{storageFilePath}' was not found in the bucket.");
            }
            
            await _storageClient.UpdateObjectAsync(storageObject, new UpdateObjectOptions
            {
                PredefinedAcl = PredefinedObjectAcl.PublicRead
            });

            return $"https://storage.googleapis.com/{BucketName}/{storageFilePath}";
        }
        catch (Exception ex)
        {
            Log.Error("An error occurred while making the file public: {ExMessage}", 
                ex.Message);
            throw;
        }
    }

    public Task<DownloadLibraryVersionDTO> DownloadAsync(string storageFilePath)
    {
        if (string.IsNullOrEmpty(storageFilePath))
        {
            throw new ArgumentException("Storage file path cannot be null or empty.");
        }
        
        try
        {
            var fileStream = new MemoryStream();
            
            var downloadedObject = _storageClient.DownloadObject(BucketName, storageFilePath, fileStream);
            if (downloadedObject == null)
            {
                throw new FileNotFoundException
                    ($"The file at path '{storageFilePath}' was not found in the bucket.");
            }
            
            var downloadedFileName = Path.GetFileName(downloadedObject.Name);
            var fileContent = fileStream.GetBuffer();

            return Task.FromResult(new DownloadLibraryVersionDTO
            {
                FileContent = fileContent,
                ContentType = downloadedObject.ContentType,
                FileName = downloadedFileName
            });
        }
        catch (Exception ex)
        {
            Log.Error("An error occurred while downloading the file: {ExMessage}", 
                ex.Message);
            throw;
        }
    }

    public void Dispose()
    {
        if (_firebaseApp == null)
            return;
        
        try
        {
            _firebaseApp.Delete();
        }
        catch (Exception ex)
        {
            Log.Error("Error occurred while disposing Firebase App: {ExMessage}", 
                ex.Message);
        }

        _firebaseApp = null;
    }
}