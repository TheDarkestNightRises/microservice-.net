using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase 
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;

    public PlatformsController(IPlatformRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetAllPlatformsAsync()
    {
        var platforms = await _repository.GetAllPlatformsAsync();
        var platformsMapped = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
        return Ok(platformsMapped);
    }

    [HttpGet("{id}", Name = "GetPlatformByIdAsync")]
    public async Task<ActionResult<PlatformReadDto>> GetPlatformByIdAsync(int id)
    {
        var platformItem = await _repository.GetPlaformByIdAsync(id);
        if (platformItem is not null)
        {
            var platformMapped = _mapper.Map<PlatformReadDto>(platformItem);
            return Ok(platformMapped);
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatformAsync(PlatformCreateDto platformCreateDto) 
    {
        var platformModel = _mapper.Map<Platform>(platformCreateDto);
        await _repository.CreatePlatformAsync(platformModel);
        await _repository.SaveChangesAsync();
        
        var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
        
        return CreatedAtRoute(nameof(GetPlatformByIdAsync), new {Id = platformReadDto.Id}, platformReadDto);
    }
}