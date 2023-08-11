using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }

        //GET Walks
        // Get: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=1
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var allWalks = await _walkRepository.GetAllWalks(filterOn, filterQuery,
                sortBy, isAscending ?? true,
                pageNumber, pageSize);

            // Create an exception
            //throw new Exception("This is a new exception");


            var walkDto = _mapper.Map<List<WalkDTO>>(allWalks);

            return Ok(walkDto);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkExists = await _walkRepository.GetByIdAsync(id);

            if (walkExists == null)
            {
                return BadRequest();
            }

            var walkDto = _mapper.Map<WalkDTO>(walkExists);

            return Ok(walkDto);

        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDTO addWalkRequestDTO)
        {
                var walkModel = _mapper.Map<Walk>(addWalkRequestDTO);

                await _walkRepository.CreateAsync(walkModel);


                return Ok(_mapper.Map<WalkDTO>(walkModel));
        }

        [HttpPut("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, UpdateWalkDTO updateWalkDTO)
        {
           
                var walkModel = _mapper.Map<Walk>(updateWalkDTO);

                walkModel = await _walkRepository.UpdateWalkAsync(id, walkModel);

                if (walkModel == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<WalkDTO>(walkModel));
            

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalk([FromRoute] Guid id)
        {
            var walkToDelete = await _walkRepository.DeleteAsync(id);

            if(walkToDelete == null)
            {
                return NotFound();
            }


            return Ok(_mapper.Map<WalkDTO>(walkToDelete));
        }
    }
}
