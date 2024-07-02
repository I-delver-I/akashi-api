using System.Text.Json;
using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Security;
using Microsoft.EntityFrameworkCore;

namespace AkaShi.Infrastructure.Context;

public static class ModelBuilderExtensions
{
    public static void Configure(this ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.NoAction;
        }
        
        modelBuilder.Entity<User>()
            .HasAlternateKey(u => u.Username);
        modelBuilder.Entity<User>()
            .HasAlternateKey(u => u.Email);
        
        modelBuilder.Entity<LibraryVersion>()
            .HasIndex(lv => new { lv.LibraryId, lv.Name })
            .IsUnique();

        modelBuilder.Entity<Library>()
            .HasAlternateKey(l => l.Name);
        
        /*modelBuilder.Entity<Library>()
            .Property(l => l.DownloadsCount)
            .HasDefaultValue(0);*/
        
        modelBuilder.Entity<FileExtension>()
            .HasIndex(f => f.Name)
            .IsUnique();
        
        modelBuilder.Entity<LibraryVersionDependency>()
            .HasIndex(p => new { p.FrameworkId, p.LibraryVersionId, p.DependencyLibraryId })
            .IsUnique();

        modelBuilder.Entity<LibraryVersionSupportedFramework>()
            .HasIndex(p => new { p.FrameworkId, p.LibraryVersionId })
            .IsUnique();
        
        modelBuilder.Entity<Framework>()
            .HasAlternateKey(f => f.VersionName);
    }
    
    public static void Seed(this ModelBuilder modelBuilder)
    {
        var users = CreateUsers();
        var frameworks = GetDataFromJson<Framework>("./SeedData/framework.json");
        var fileExtensions = GetDataFromJson<FileExtension>("./SeedData/fileExtension.json");
        var libraries = GetDataFromJson<Library>("./SeedData/library.json");
        var libraryVersions = GetDataFromJson<LibraryVersion>("./SeedData/libraryVersion.json");
        var images = GetDataFromJson<Image>("./SeedData/image.json");
        var libraryVersionDependencies = 
            GetDataFromJson<LibraryVersionDependency>("./SeedData/libraryVersionDependency.json");
        var libraryVersionSupportedFrameworks = 
            GetDataFromJson<LibraryVersionSupportedFramework>("./SeedData/libraryVersionSupportedFramework.json");
        var libraryArchiveHashes = 
            GetDataFromJson<LibraryArchiveHash>("./SeedData/libraryArchiveHash.json");
        
        modelBuilder.Entity<Framework>().HasData(frameworks);
        modelBuilder.Entity<FileExtension>().HasData(fileExtensions);
        modelBuilder.Entity<Library>().HasData(libraries);
        modelBuilder.Entity<LibraryVersion>().HasData(libraryVersions);
        modelBuilder.Entity<Image>().HasData(images);
        modelBuilder.Entity<User>().HasData(users);
        modelBuilder.Entity<LibraryVersionDependency>().HasData(libraryVersionDependencies);
        modelBuilder.Entity<LibraryVersionSupportedFramework>().HasData(libraryVersionSupportedFrameworks);
        modelBuilder.Entity<LibraryArchiveHash>().HasData(libraryArchiveHashes);
    }

    private static ICollection<TEntity> GetDataFromJson<TEntity>(string jsonFilePath)
    {
        var dataJson = File.ReadAllText(jsonFilePath);
        return JsonSerializer.Deserialize<ICollection<TEntity>>(dataJson);
    }

    private static IEnumerable<User> CreateUsers(int numberOfUsers = 5)
    {
        var users = new List<User>();

        for (var i = 1; i <= numberOfUsers; i++)
        {
            var salt = Convert.ToBase64String(SecurityHelper.GetDeterminedBytes());
            var password = $"passw0rd{i}";
            var hashedPassword = SecurityHelper.HashPassword(password, Convert.FromBase64String(salt));

            users.Add(new User
            {
                Id = i,
                Email = $"test{i}@gmail.com",
                PasswordHash = hashedPassword,
                PasswordSalt = salt,
                Username = $"testUser{i}"
            });
        }

        return users;
    }
}