using Microsoft.EntityFrameworkCore.Update.Internal;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllRegions();
        Task<Region?> GetById(Guid id);
        Task<Region> Create(Region region);
        Task<Region?> UpdateRegion(Guid id, Region region);
        Task<Region?> DeleteAsync(Guid id);
    }
}
