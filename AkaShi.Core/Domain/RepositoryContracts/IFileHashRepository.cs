using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts.Abstract;

namespace AkaShi.Core.Domain.RepositoryContracts;

public interface IFileHashRepository : IRepository<LibraryArchiveHash>
{
    Task<LibraryArchiveHash> GetByHashAsync(string fileHash);
}