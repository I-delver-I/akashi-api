using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts.Abstract;
using AkaShi.Core.Helpers;
using AkaShi.Core.Helpers.RepositoryParams;

namespace AkaShi.Core.Domain.RepositoryContracts;

public interface ILibraryVersionRepository : IRepository<LibraryVersion>
{
    Task<PagedList<LibraryVersion>> GetByLibraryIdAsync(LibraryVersionParams libraryVersionParams, int id);
    Task<PagedList<LibraryVersion>> GetAllAsync(LibraryVersionParams libraryVersionParams);
}