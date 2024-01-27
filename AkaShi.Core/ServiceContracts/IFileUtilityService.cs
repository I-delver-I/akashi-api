namespace AkaShi.Core.ServiceContracts;

public interface IFileUtilityService
{
    Task<string> CalculateFileHashAsync(Stream fileStream);
    Task<bool> HashExistsAsync(string fileHash);
}