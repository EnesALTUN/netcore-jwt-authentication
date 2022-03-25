using AutoMapper;
using UdemyAuthServer.Core.Dtos;
using UdemyAuthServer.Core.Entities;

namespace UdemyAuthServer.Service.AutoMapper
{
    internal class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<Product, ProductDto>()
                .ReverseMap();

            CreateMap<UserApp, UserAppDto>()
                .ReverseMap();
        }
    }
}