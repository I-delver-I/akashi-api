using AkaShi.Core.ServiceContracts;
using Mono.Cecil;

namespace AkaShi.Infrastructure.Services;

public class DllValidationService : IDllValidationService
{
    public Task<bool> IsCsharpDll(Stream dllFile)
    {
        try
        {
            using var assembly = AssemblyDefinition.ReadAssembly(dllFile);
            
            if (assembly.MainModule.Kind != ModuleKind.Dll)
            {
                return Task.FromResult(false);
            }
            
            if (assembly.EntryPoint != null)
            {
                return Task.FromResult(false);
            }
            
            var hasManagedTypes = assembly.Modules.Any(module => module.HasTypes && module.Types.Count > 0);
            
            return Task.FromResult(hasManagedTypes);
        }
        catch (BadImageFormatException)
        {
            return Task.FromResult(false);
        }
        catch (AssemblyResolutionException)
        {
            return Task.FromResult(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return Task.FromResult(false);
        }
    }
}