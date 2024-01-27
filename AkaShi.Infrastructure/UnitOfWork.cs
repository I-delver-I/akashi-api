using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace AkaShi.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction _currentTransaction;

    public IUserRepository UserRepository { get; set; }
    public ILibraryRepository LibraryRepository { get; set; }
    public IRefreshTokenRepository RefreshTokenRepository { get; set; }
    public IImageRepository ImageRepository { get; set; }
    public ILibraryVersionRepository LibraryVersionRepository { get; set; }
    public IFileExtensionRepository FileExtensionRepository { get; set; }
    public ILibraryVersionSupportedFrameworkRepository LibraryVersionSupportedFrameworkRepository { get; set; }
    public ILibraryVersionDependencyRepository LibraryVersionDependencyRepository { get; set; }
    public IFrameworkRepository FrameworkRepository { get; set; }
    public IFileHashRepository FileHashRepository { get; set; }
    
    public UnitOfWork(ApplicationDbContext context, IUserRepository userRepository, 
        ILibraryRepository libraryRepository, IRefreshTokenRepository refreshTokenRepository,
        IImageRepository imageRepository, ILibraryVersionRepository libraryVersionRepository,
        IFileExtensionRepository fileExtensionRepository, 
        ILibraryVersionSupportedFrameworkRepository libraryVersionSupportedFrameworkRepository,
        ILibraryVersionDependencyRepository libraryVersionDependencyRepository,
        IFrameworkRepository frameworkRepository, IFileHashRepository fileHashRepository)
    {
        _context = context;
        UserRepository = userRepository;
        LibraryRepository = libraryRepository;
        RefreshTokenRepository = refreshTokenRepository;
        ImageRepository = imageRepository;
        LibraryVersionRepository = libraryVersionRepository;
        FileExtensionRepository = fileExtensionRepository;
        LibraryVersionSupportedFrameworkRepository = libraryVersionSupportedFrameworkRepository;
        LibraryVersionDependencyRepository = libraryVersionDependencyRepository;
        FrameworkRepository = frameworkRepository;
        FileHashRepository = fileHashRepository;
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return _currentTransaction ??= await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            if (_currentTransaction != null)
            {
                await _context.SaveChangesAsync();
                await _currentTransaction.CommitAsync();
            }
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            _currentTransaction = null;
        }
    }
    
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        await _context.DisposeAsync();
    }
}