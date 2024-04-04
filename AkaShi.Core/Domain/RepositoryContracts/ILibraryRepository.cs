using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts.Abstract;
using AkaShi.Core.Helpers;
using AkaShi.Core.Helpers.RepositoryParams;

namespace AkaShi.Core.Domain.RepositoryContracts;

public interface ILibraryRepository : IRepository<Library>
{
    Task<IEnumerable<Library>> GetByUserIdAsync(int userId);
    Task<Library> GetByNameAsync(string name);
    Task<IEnumerable<Library>> GetTopDownloadedLibrariesAsync(int count);
    Task<PagedList<Library>> GetAllAsync(LibraryParams libraryParams);
}