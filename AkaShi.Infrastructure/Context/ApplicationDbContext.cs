using AkaShi.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AkaShi.Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; private set; }
    public DbSet<Image> Images { get; private set; }
    public DbSet<Library> Libraries { get; private set; }
    public DbSet<Framework> Frameworks { get; private set; }
    public DbSet<RefreshToken> RefreshTokens { get; private set; }
    public DbSet<LibraryVersion> LibraryVersions { get; private set; }
    public DbSet<LibraryVersionDependency> LibraryVersionDependencies { get; private set; }
    public DbSet<LibraryVersionSupportedFramework> LibraryVersionSupportedFrameworks { get; private set; }
    public DbSet<FileExtension> FileExtensions { get; private set; }
    public DbSet<LibraryArchiveHash> LibraryArchiveHashes { get; private set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Setting up entities using extension method
        modelBuilder.Configure();
        
        // Seeding data using extension method
        // NOTE: this method will be called every time after adding a new migration, cuz we use Bogus for seed data
        modelBuilder.Seed();
    }
}