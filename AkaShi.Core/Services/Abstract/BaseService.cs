using AkaShi.Core.Domain.RepositoryContracts;
using AutoMapper;

namespace AkaShi.Core.Services.Abstract;

public abstract class BaseService
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IMapper Mapper;

    protected BaseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        UnitOfWork = unitOfWork;
        Mapper = mapper;
    }
}