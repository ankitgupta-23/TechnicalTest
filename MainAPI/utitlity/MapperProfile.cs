using AutoMapper;
using MainAPI.DTOs;
using DataLayer.Entities;

namespace MainAPI.utitlity
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<EntityDTO, Entity>().ReverseMap();
            CreateMap<NameDTO, Name>().ReverseMap();
            CreateMap<AddressDTO, Address>().ReverseMap();
            CreateMap<DatesDTO, Date>().ReverseMap();
        }
    }
}
