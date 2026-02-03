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
                .ForMember(s => s.Roles, opt => opt.Ignore())
                .ForMember(s => s.Permissions, opt => opt.Ignore())
                .ForMember(s => s.Services, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RoleEntity, RoleDTO>().ForMember(d => d.Permissions, o => o.MapFrom(s => s.RolePermissions.Select(rp => rp.Permissions)));

            CreateMap<RoleDTO, RoleEntity>();

            CreateMap<PermissionEntity, PermissionDTO>().ReverseMap();

            CreateMap<ServiceEntity, ServiceDTO>().ReverseMap();

            CreateMap<UserRequest, UserDTO>(MemberList.Source)
                .ForMember(s => s.IsRequiredChangePassword, opt => opt.MapFrom(src => src.ChangedPasswordFirstTime))
                .ReverseMap();

            CreateMap<UserDTO, CreateOrUpdateUserRequest> ().ReverseMap();
        }
    }
}

