namespace AkaShi.Core.ServiceContracts;

public interface ILibraryArchiveValidationService
{
    Task<bool> ValidateArchiveAsync(Stream archiveStream, string archiveFullName, string libraryName, 
        string libraryVersionName);
}