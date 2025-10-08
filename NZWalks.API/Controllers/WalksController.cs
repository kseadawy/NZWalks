using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
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
        public async Task<ActionResult> GetAll()
        {
            var walkDomainList = await _walkRepository.GetAllAsync(w=>w.Region, w=>w.Difficulty);
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
            var walkDomain = await _walkRepository.GetByIdAsync(id);
            if (walkDomain == null)
                return BadRequest();

            walkDomain = _mapper.Map(updateWalkDto, walkDomain);

            await _walkRepository.UpdateAsync(walkDomain);
            var walkDto = _mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
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
