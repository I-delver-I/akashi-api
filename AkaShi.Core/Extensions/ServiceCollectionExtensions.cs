using System.Reflection;
using AkaShi.Core.Auth;
using AkaShi.Core.JWT;
using AkaShi.Core.MappingProfiles;
using AkaShi.Core.ServiceContracts;
using AkaShi.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AkaShi.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCore(this IServiceCollection services)
    {
        services.AddScoped<JwtIssuerOptions>();
        services.AddScoped<JwtFactory>();
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ILibraryService, LibraryService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<ILibraryVersionService, LibraryVersionService>();
        services.AddScoped<ILibraryVersionDependencyService, LibraryVersionDependencyService>();
        services.AddScoped<ILibraryVersionSupportedFrameworkService, LibraryVersionSupportedFrameworkService>();
        services.AddScoped<IStatisticsService, StatisticsService>();
    }
    
    public static void RegisterAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<LibraryProfile>();
                cfg.AddProfile<ImageProfile>();
                cfg.AddProfile<FrameworkProfile>();
            },
            Assembly.GetExecutingAssembly());
    }
}