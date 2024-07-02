using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts.Abstract;
using AkaShi.Core.Helpers;
using AkaShi.Core.Helpers.RepositoryParams;

namespace AkaShi.Core.Domain.RepositoryContracts;

public interface ILibraryVersionRepository : IRepository<LibraryVersion>
{
    Task<IEnumerable<LibraryVersion>> GetByLibraryIdAsync(int id);
    Task<PagedList<LibraryVersion>> GetAllAsync(LibraryVersionParams libraryVersionParams);
}