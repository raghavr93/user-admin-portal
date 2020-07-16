using AutoMapper;
using UserAdminApi.Model;
using UserAdminApi.Model.Dto;

namespace UserAdminApi
{
    internal class UserMappings:Profile
    {
        public UserMappings()
        {
            CreateMap<User, AuthUserDto>().ReverseMap();
            CreateMap<User, RegisterUserDto>().ReverseMap();
            CreateMap<User, GetUserDto>().ReverseMap();
            CreateMap<User, TokenDto>().ReverseMap();
        }
     
    }
}