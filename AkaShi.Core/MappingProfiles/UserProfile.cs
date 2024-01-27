using AkaShi.Core.Domain.Entities;
using AkaShi.Core.DTO.User;
using AutoMapper;

namespace AkaShi.Core.MappingProfiles;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>();
        CreateMap<UserRegisterDTO, User>();
    }
}