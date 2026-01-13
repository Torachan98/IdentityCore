using System.Runtime.CompilerServices;
using AutoMapper;
using IdentityCore.Business.Interfaces;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Entities;
using IdentityCore.EFs.Requests;
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
        public RoleBusiness(IMapper mapper, IUnitOfWork unitOfWork, IRoleRepository roleRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
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
                pageSize = int.Parse(request.PageSize);
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
            var isExistedRole = await _roleRepository.Get().FirstOrDefaultAsync(s => s.GUID == request.GUID);
            if (isExistedRole == null) 
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Not found role");
            }

            var isExistedRoleName = await _roleRepository.Get().FirstOrDefaultAsync(s => s.Name == request.Name);
            if(isExistedRoleName != null)
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Name is already existed");
            }

            isExistedRole.Description = request.Description;
            isExistedRole.DateModified = DateTime.UtcNow;
            return _mapper.Map<RoleDTO>(isExistedRoleName);
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
