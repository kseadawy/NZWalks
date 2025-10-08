using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //**********Region Mappings**********
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>();
            //**********Walk Mappings**********
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<AddWalkRequestDto, Walk>();
            CreateMap<UpdateWalkRequestDto, Walk>();
            //**********Difficulty Mappings**********
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
            
        }
    }
}
