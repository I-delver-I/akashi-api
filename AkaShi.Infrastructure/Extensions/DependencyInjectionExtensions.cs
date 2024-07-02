using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Core.ServiceContracts;
using AkaShi.Infrastructure.Repositories;
using AkaShi.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AkaShi.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IFirebaseStorageService, FirebaseStorageService>();
        services.AddSingleton<IDllValidationService, DllValidationService>();

        services.AddScoped<IFileUtilityService, FileUtilityService>();
        services.AddScoped<ILibraryArchiveValidationService, LibraryArchiveValidationService>();
        services.AddScoped<ILibraryArchiveExtractorService, LibraryArchiveExtractorService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ILibraryRepository, LibraryRepository>();
        services.AddScoped<ILibraryVersionRepository, LibraryVersionRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<IFileExtensionRepository, FileExtensionRepository>();
        services.AddScoped<ILibraryVersionDependencyRepository, LibraryVersionDependencyRepository>();
        services.AddScoped<IFileHashRepository, FileHashRepository>();
        services.AddScoped<IFrameworkRepository, FrameworkRepository>();
        services.AddScoped<ILibraryVersionSupportedFrameworkRepository, LibraryVersionSupportedFrameworkRepository>();
    }
}