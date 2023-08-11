using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    // https://localhost:1234/api/regions 
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }
        
        // GET : https://localhost:1234/api/regions
        [HttpGet]
        //[Authorize]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
  
            
            // Get Data from Database - Domain models 
            var regionsDomain = await _regionRepository.GetAllRegions ();

           

            var regionsDto = _mapper.Map<List<RegionDTO>>(regionsDomain);

            // Return DTO


            return Ok(regionsDto);

        }


        [HttpGet("{id:Guid}")]
        //[Authorize]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            var regionDomain = await _regionRepository.GetById(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RegionDTO>(regionDomain));
        }

        //POST To Create New Region
        // POST : https://localhost:portnumber/api/regions

        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] RegionRequestDto regionRequestDto)
        {
            
                // Map or Convert DTO to Domain Model
                var regionDomainModel = _mapper.Map<Region>(regionRequestDto);
                // Use Domain Model to create Region
                regionDomainModel = await _regionRepository.Create(regionDomainModel);

                // Map Domain Model back to DTO

                var regionDto = _mapper.Map<RegionDTO>(regionDomainModel);

                return Ok(regionDto);
           

        }



        [HttpPut("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionDto)
        {
            
                var regionDomainModel = _mapper.Map<Region>(updateRegionDto);

                // Check if region exists
                regionDomainModel = await _regionRepository.UpdateRegion(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                var regionDto = _mapper.Map<RegionDTO>(regionDomainModel);


                return Ok(regionDto);
          
           
        }



        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteRegion(Guid id)
        {
            var regionDomainModel = await _regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            var regionDto = _mapper.Map<RegionDTO>(regionDomainModel);


            return Ok(regionDto);
        }

    }
}
