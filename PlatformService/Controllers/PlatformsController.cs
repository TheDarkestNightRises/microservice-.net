using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformsController(IPlatformRepository repository, IMapper mapper, ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
    {
        _repository = repository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
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

        //Send Sync Message
        try
        {
            await _commandDataClient.SendPlatformToCommand(platformReadDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not send data: {e.Message}");
        }

        //Send Async Message
        try
        {
            var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
            platformPublishedDto.Event = "Platform_Published";
            _messageBusClient.PublishNewPlatform(platformPublishedDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not send asynchronously : {e.Message}");
        }

        return CreatedAtRoute(nameof(GetPlatformByIdAsync), new { Id = platformReadDto.Id }, platformReadDto);
    }
}