using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    //https://localhost:portnumber/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        // GET: api/Regions
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var regionDomainList = await _regionRepository.GetAllAsync();
            var regionDtos = _mapper.Map<List<RegionDto>>(regionDomainList);
            return Ok(regionDtos);
        }

        // GET: api/Regions/5
        [HttpGet(template:"{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var regionDomain = await _regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            
            var regionDto = _mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }

        // PUT: api/Regions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> EditRegion(Guid id, UpdateRegionRequestDto updateRegionDto)
        {
                var regionDomain = await _regionRepository.GetByIdAsync(id);
                if (regionDomain == null)
                    return BadRequest();

                regionDomain.Code = updateRegionDto.Code;
                regionDomain.Name = updateRegionDto.Name;
                regionDomain.RegionImageUrl = updateRegionDto.RegionImageUrl;

                await _regionRepository.UpdateAsync(regionDomain);
                var regionDto = _mapper.Map<RegionDto>(regionDomain);
                return Ok(regionDto);
        }

        // POST: api/Regions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> CreateRegion(AddRegionRequestDto regionRequestDto)
        {
            var regionDomain = _mapper.Map<Region>(regionRequestDto);
            await _regionRepository.AddAsync(regionDomain);

            var regionDto = _mapper.Map<RegionDto>(regionDomain);
            return CreatedAtAction("GetById", new { id = regionDomain.Id }, regionDto);
        }

        // DELETE: api/Regions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegion(Guid id)
        {
            var region = await _regionRepository.GetByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            await _regionRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
