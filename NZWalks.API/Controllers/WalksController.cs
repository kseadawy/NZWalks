using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    //https://localhost:portnumber/api/Walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }

        // GET: api/Walks
        [HttpGet]
        public async Task<ActionResult> GetAll(
            [FromQuery]string? filterBy, [FromQuery]string? filterQuery, 
            [FromQuery] string? sortBy, [FromQuery] bool isAscending=true,
            [FromQuery]int pageNumber=1, [FromQuery] int pageSize=100)
        {
            bool checkFilter = !String.IsNullOrEmpty(filterBy) && filterBy.Equals("name", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(filterQuery);
            bool checkSort = !String.IsNullOrEmpty(sortBy) && (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase) || sortBy.Equals("lengthinkm", StringComparison.OrdinalIgnoreCase));
           
            
            var walkDomainList = await _walkRepository.GetAllAsync(
                !checkFilter ? null : w => w.Name.Contains(filterQuery), 
                sortBy: checkSort? sortBy:null, ascending: isAscending,
                pageNumber: pageNumber, pageSize: pageSize,
                w => w.Region, w => w.Difficulty);
            
            var walkDtos = _mapper.Map<List<WalkDto>>(walkDomainList);
            return Ok(walkDtos);
        }

        // GET: api/Walks/5
        [HttpGet(template:"{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var walkDomain = await _walkRepository.GetByIdAsync(id, w=>w.Region,w=>w.Difficulty);

            if (walkDomain == null)
            {
                return NotFound();
            }

            
            var walkDto = _mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }

        // PUT: api/Walks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditWalk(Guid id, UpdateWalkRequestDto updateWalkDto)
        {
            if (ModelState.IsValid)
            {
                var walkDomain = await _walkRepository.GetByIdAsync(id);
                if (walkDomain == null)
                    return BadRequest();

                walkDomain = _mapper.Map(updateWalkDto, walkDomain);

                await _walkRepository.UpdateAsync(walkDomain);
                var walkDto = _mapper.Map<WalkDto>(walkDomain);
                return Ok(walkDto);
            }
            return BadRequest(ModelState);
        }

        // POST: api/Walks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> CreateWalk(AddWalkRequestDto WalkRequestDto)
        {
            var walkDomain = _mapper.Map<Walk>(WalkRequestDto);
            await _walkRepository.AddAsync(walkDomain);

            var walkDto = _mapper.Map<WalkDto>(walkDomain);
            return CreatedAtAction("GetById", new { id = walkDomain.Id }, walkDto);
        }

        // DELETE: api/Walks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            var walk = await _walkRepository.GetByIdAsync(id);
            if (walk == null)
            {
                return NotFound();
            }

            await _walkRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
