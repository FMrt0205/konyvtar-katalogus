using Konyvtar_katalogus.Models;

namespace Konyvtar_katalogus.Services
{
    public interface IStatisticsService
    {
        Task<LibraryStatistics> GenerateStatisticsAsync();
    }
}
