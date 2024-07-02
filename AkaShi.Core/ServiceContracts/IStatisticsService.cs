using AkaShi.Core.DTO.Library;

namespace AkaShi.Core.ServiceContracts;

public interface IStatisticsService
{
    Task<ICollection<LibraryDownloadStatsDTO>> GetTopDownloadsAsync(int count);
}