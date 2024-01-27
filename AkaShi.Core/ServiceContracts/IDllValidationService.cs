namespace AkaShi.Core.ServiceContracts;

public interface IDllValidationService
{
    Task<bool> IsCsharpDll(Stream dllFile);
}