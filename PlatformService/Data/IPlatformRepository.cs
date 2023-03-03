using PlatformService.Models;

namespace PlatformService.Data;

public interface IPlatformRepository 
{
    Task<bool> SaveChangesAsync();

    Task<IEnumerable<Platform>> GetAllPlatformsAsync();
    Task<Platform> GetPlaformByIdAsync(int id);
    Task CreatePlatformAsync(Platform platform);
}