using Core;
using AutoMapper;
using Core.Entities;
using Core.Dtos.JWT;
namespace Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, RegisterDto>().ReverseMap();

        }

    }
}
