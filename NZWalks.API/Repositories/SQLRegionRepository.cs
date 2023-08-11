using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Region> Create(Region region)
        {
            await _dbContext.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var regionExists = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

            if (regionExists == null)
            {
                return null;
            }


            _dbContext.Regions.Remove(regionExists);
            await _dbContext.SaveChangesAsync();

            return regionExists;
        }

        public async Task<List<Region>> GetAllRegions()
        {
           return await _dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetById(Guid id)
        {
            return await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Region?> UpdateRegion(Guid id, Region region)
        {
            var regionExists = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if (regionExists == null)
            {
                return null;
            }

            regionExists.Code = region.Code;
            regionExists.Name = region.Name;
            regionExists.RegionImageUrl = region.RegionImageUrl;

            await _dbContext.SaveChangesAsync();

            return regionExists;
        }
    }
}
