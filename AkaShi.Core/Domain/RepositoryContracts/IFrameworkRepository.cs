using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts.Abstract;

namespace AkaShi.Core.Domain.RepositoryContracts;

public interface IFrameworkRepository : IRepository<Framework>
{
    Task<Framework> GetByVersionNameAsync(string name);
}