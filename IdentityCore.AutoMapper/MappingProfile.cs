using AutoMapper;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Entities;
using IdentityCore.EFs.Enums;
using IdentityCore.EFs.Helpers;
using IdentityCore.EFs.Requests;

namespace IdentityCore.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<UserEntity, UserDTO>()
                .ForMember(s => s.FullName, opt => opt.MapFrom(src => src.FirstName + src.MiddleName + src.LastName))
                .ForMember(s => s.GroupPermissions, opt => opt.Ignore())
                .ForMember(s => s.GroupRoles, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RoleEntity, RoleDTO>().ReverseMap();

            CreateMap<PermissionEntity, PermissionDTO>()
                .ReverseMap();

            CreateMap<UserRequest, UserDTO>(MemberList.Source)
                .ForMember(s => s.IsRequiredChangePassword, opt => opt.MapFrom(src => src.ChangedPasswordFirstTime))
                .ForMember(s => s.GroupPermissions, opt => opt.Ignore())
                .ForMember(s => s.GroupRoles, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}

