using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CategoryEntity, CategoryDto>();
        }
    }
}
