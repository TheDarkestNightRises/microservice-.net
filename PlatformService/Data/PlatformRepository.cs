using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public class PlatformRepository : IPlatformRepository
{
    private AppDbContext _context;

    public PlatformRepository(AppDbContext context)
    {
        _context = context;    
    }

    public async Task CreatePlatformAsync(Platform platform)
    {
        if (platform == null) 
        {
            throw new ArgumentNullException(nameof(platform));
        }
        await _context.Platforms.AddAsync(platform);
    }
    

    public async Task<IEnumerable<Platform>> GetAllPlatformsAsync()
    {
        return await _context.Platforms.ToListAsync();
    }

    public async Task<Platform> GetPlaformByIdAsync(int id)
    {
        return await _context.Platforms.FirstAsync(p => p.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}