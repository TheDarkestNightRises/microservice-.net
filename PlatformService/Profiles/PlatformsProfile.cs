namespace PlatformService.Profile;
using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

public class PlatformsProfile : Profile
{
    public PlatformsProfile()
    {
        //Source --> Target
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformCreateDto, Platform>();
        CreateMap<PlatformReadDto, PlatformPublishedDto>();
    }
}