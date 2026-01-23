using System.Runtime.CompilerServices;
using AutoMapper;
using IdentityCore.Business.Interfaces;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Entities;
using IdentityCore.EFs.Requests;
using IdentityCore.Repository;
using IdentityCore.Repository.Interfaces;
using IdentityCore.Repository.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace IdentityCore.Business
{
    public class RoleBusiness : BaseBusiness, IRoleBusiness
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IPermissionRepository _permissionRepository;
        public RoleBusiness(IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IRoleRepository roleRepository, 
            IUserRepository userRepository, 
            IUserRoleRepository userRoleRepository,
            IRolePermissionRepository rolePermissionRepository,
            IPermissionRepository permissionRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<PaginationItems<RoleDTO>> GetRoles(RoleFetchRequest request)
        {
            var roleQuery = _roleRepository.Get(s => !s.IsDeleted);
            int pageNum = 1;
            int pageSize = 30;
            int totalCount = roleQuery.Count(s => !s.IsDeleted);

            if (!string.IsNullOrEmpty(request.PageNum) && !string.IsNullOrEmpty(request.PageSize))
            {
                pageNum = int.Parse(request.PageNum);
                
                if (request.PageSize == "MAX")
                {
                    pageSize = totalCount;
                }
                else
                {
                    pageSize = int.Parse(request.PageSize);
                }
            }

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                roleQuery = roleQuery.Where(s => s.Name.Contains(request.Keyword));
            }

            if (request.IsLock.HasValue)
            {
                roleQuery = roleQuery.Where(s => s.Name.Contains(request.Keyword));
            }

            

            roleQuery = roleQuery.Skip((pageNum - 1) * pageSize).Take(pageSize);

            var roleEntities = await roleQuery
                                        .Include(s => s.RolePermissions)
                                        .ThenInclude(p => p.Permissions)
                                        .ToListAsync();
            
            return new PaginationItems<RoleDTO>(pageSize, pageNum, totalCount, _mapper.Map<List<RoleDTO>>(roleEntities));
        }

        public async Task<RoleDTO> GetRoleByIdAsync(Guid guid)
        {
            var result = await _roleRepository
                                    .Get(s => s.GUID == guid)
                                    .Include(s => s.RolePermissions)
                                    .ThenInclude(p => p.Permissions)
                                    .FirstOrDefaultAsync();

            return _mapper.Map<RoleDTO>(result);
        }

        public async Task<RoleDTO> CreateRolesAsync(CreateOrUpdateRoleRequest request)
        {
            var isExistedRole = await _roleRepository.Get().FirstOrDefaultAsync(s => s.Name.Contains(request.Name));
            if (isExistedRole != null) 
            {
                if (!isExistedRole.IsDeleted)
                {
                    isExistedRole.IsDeleted = false;
                    _roleRepository.Update(isExistedRole, s => s.IsDeleted);
                    await _unitOfWork.CommitAsync();
                    return _mapper.Map<RoleDTO>(isExistedRole);
                }
            }

            var roleEntities = new RoleEntity()
            {
                Name = request.Name,
                Description = request.Description,
                IsLock = request.IsLock,
            };

            var result = _roleRepository.Add(roleEntities);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<RoleDTO>(result);
        }

        public async Task<RoleDTO> UpdateRolesAsync(CreateOrUpdateRoleRequest request)
        {
            var roleItem = await _roleRepository.Get().FirstOrDefaultAsync(s => s.GUID == request.GUID);
            if (roleItem == null) 
            {
                throw new FriendlyException(StatusCodes.Status404NotFound, "Not found role");
            }

            //var isExistedRoleName = await _roleRepository.Get().FirstOrDefaultAsync(s => s.Name == request.Name);
            //if(isExistedRoleName != null)
            //{
            //    throw new FriendlyException(StatusCodes.Status400BadRequest, "Name is already existed");
            //}

            var permissions = await _permissionRepository
                                        .Get(s => request.Permissions.Any(r => s.GUID == r))
                                        .ToListAsync();

            roleItem.Description = request.Description;
            roleItem.DateModified = DateTime.UtcNow;

            _rolePermissionRepository.DeleteWhere(s => s.RoleId == roleItem.RoleId, true);
            _rolePermissionRepository.AddRange(permissions.Select(s => new RolePermissionEntity()
            {
                RoleId = roleItem.RoleId,
                PermissionId = s.PermissionId
            }).ToList());

            _roleRepository.Update(roleItem);

            await _unitOfWork.CommitAsync();

            var roleDto = _mapper.Map<RoleDTO>(roleItem);
            roleDto.Permissions.Clear();
            roleDto.Permissions.AddRange(_mapper.Map<List<PermissionDTO>>(permissions));

            return roleDto;
        }

        public async Task<List<RoleDTO>> AssignUserRolesAsync(List<Guid> RoleIds, int UserId)
        {
            var roles = await _roleRepository
                                        .Get(s => RoleIds.Any(r => r == s.GUID))
                                        .ToListAsync();
            if (roles.Count == 0 || roles.Count != RoleIds.Count) 
            {
                throw new FriendlyException(StatusCodes.Status404NotFound, "Role not existed");
            }

            _userRoleRepository.DeleteWhere(s => s.UserId == UserId, true);

            _userRoleRepository.AddRange(roles.Select(s => new UserRoleEntity()
            {
                RoleId = s.RoleId,
                UserId = UserId
            }).ToList());

            await _unitOfWork.CommitAsync();

            return _mapper.Map<List<RoleDTO>>(roles);

        }

        public async Task<bool> DeleteRolesAsync(Guid guid)
        {
            var isExistedRole = await _roleRepository.Get().FirstOrDefaultAsync(s => s.GUID == guid);
            if (isExistedRole == null) 
            {
                return false;
            }

            _roleRepository.DeleteWhere(s => s.GUID == guid);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
