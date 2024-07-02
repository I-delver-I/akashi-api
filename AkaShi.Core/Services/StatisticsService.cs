using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Core.DTO.Library;
using AkaShi.Core.ServiceContracts;
using AkaShi.Core.Services.Abstract;
using AutoMapper;

namespace AkaShi.Core.Services;

public class StatisticsService : BaseService, IStatisticsService
{
    private readonly ILibraryRepository _libraryRepository;
    
    public StatisticsService(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork, mapper)
    {
        _libraryRepository = UnitOfWork.LibraryRepository;
    }

    public async Task<ICollection<LibraryDownloadStatsDTO>> GetTopDownloadsAsync(int count)
    {
        var topLibraries = await _libraryRepository.GetTopDownloadedLibrariesAsync(count);
        
        return topLibraries
            .Select(lib => new LibraryDownloadStatsDTO 
            { 
                LibraryName = lib.Name, 
                DownloadsCount = lib.DownloadsCount 
            })
            .ToList();
    }
}