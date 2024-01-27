using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts.Abstract;

namespace AkaShi.Core.Domain.RepositoryContracts;

public interface ILibraryVersionRepository : IRepository<LibraryVersion>
{
    Task<IEnumerable<LibraryVersion>> GetByLibraryIdAsync(int id);
}