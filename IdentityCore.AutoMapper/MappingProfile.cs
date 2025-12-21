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
                .ForMember(s => s.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.MiddleName + " " + src.LastName))
                .ForMember(s => s.GroupPermissions, opt => opt.Ignore())
                .ForMember(s => s.GroupRoles, opt => opt.Ignore())
                .ForMember(s => s.Roles, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RoleEntity, RoleDTO>().ReverseMap();

            CreateMap<PermissionEntity, PermissionDTO>().ReverseMap();

            CreateMap<ServiceEntity, ServiceDTO>().ReverseMap();

            CreateMap<UserRequest, UserDTO>(MemberList.Source)
                .ForMember(s => s.IsRequiredChangePassword, opt => opt.MapFrom(src => src.ChangedPasswordFirstTime))
                .ForMember(s => s.GroupPermissions, opt => opt.Ignore())
                .ForMember(s => s.GroupRoles, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UserDTO, CreateOrUpdateUserRequest>()
                .ForMember(s => s.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(s => s.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl))
                .ForMember(s => s.GUID, opt => opt.MapFrom(src => src.GUID))
                .ForMember(s => s.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(s => s.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(s => s.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(s => s.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(s => s.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(s => s.PhoneCode, opt => opt.MapFrom(src => src.PhoneCode))
                .ForMember(s => s.Region, opt => opt.MapFrom(src => src.Region))
                .ReverseMap();
        }
    }
}

