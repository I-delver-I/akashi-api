using Microsoft.EntityFrameworkCore.Storage;

namespace AkaShi.Core.Domain.RepositoryContracts;

public interface IUnitOfWork : IAsyncDisposable
{
    IUserRepository UserRepository { get; set; }
    ILibraryRepository LibraryRepository { get; set; }
    IRefreshTokenRepository RefreshTokenRepository { get; set; }
    IImageRepository ImageRepository { get; set; }
    ILibraryVersionRepository LibraryVersionRepository { get; set; }
    IFileExtensionRepository FileExtensionRepository { get; set; }
    ILibraryVersionSupportedFrameworkRepository LibraryVersionSupportedFrameworkRepository { get; set; }
    ILibraryVersionDependencyRepository LibraryVersionDependencyRepository { get; set; }
    IFrameworkRepository FrameworkRepository { get; set; }
    IFileHashRepository FileHashRepository { get; set; }

    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    
    Task SaveAsync();
}