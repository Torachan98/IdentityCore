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
    public class PermissionBusiness : BaseBusiness, IPermissionBusiness
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserPermissionRepository _userPermissionRepository;

        public PermissionBusiness(IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IPermissionRepository permissionRepository, 
            IUserPermissionRepository userPermissionRepository)
        {
             _mapper = mapper;
            _unitOfWork = unitOfWork;
            _permissionRepository = permissionRepository;
            _userPermissionRepository = userPermissionRepository;
        }

        public async Task<PaginationItems<PermissionDTO>> GetPermissionsAsync(PermissionFetchRequest request)
        {
            var permissionQuery = _permissionRepository.Get(s => !s.IsDeleted);
            int pageNum = 1;
            int pageSize = 30;
            int totalCount = permissionQuery.Count(s => !s.IsDeleted);

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                permissionQuery = permissionQuery.Where(s => s.Name.Contains(request.Keyword));
            }

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

            permissionQuery = permissionQuery.Skip((pageNum - 1) * pageSize).Take(pageSize);

            var permissionEntities = await permissionQuery.ToListAsync();

            return new PaginationItems<PermissionDTO>(pageSize, pageNum, totalCount, _mapper.Map<List<PermissionDTO>>(permissionEntities));
        }

        public async Task<PermissionDTO> CreatePermissionsAsync(CreateOrUpdatePermissionRequest request)
        {
            var permissionExisted = await _permissionRepository.Get(s => s.IsDeleted && s.Name.Contains(request.Name)).FirstOrDefaultAsync();
            if (permissionExisted == null)
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Permission already existed");
            }

            var permissionEntities = _permissionRepository.Add(new PermissionEntity()
            {
                Value = request.Value,
                Name = request.Name,
                Description = request.Description,
            });

            await _unitOfWork.CommitAsync();
            return _mapper.Map<PermissionDTO>(permissionEntities);            
        }

        public async Task<PermissionDTO> UpdatePermissionsAsync(CreateOrUpdatePermissionRequest request)
        {
            var permissonEntity = await _permissionRepository.Get(s=> s.GUID == request.GUID || !s.IsDeleted).FirstOrDefaultAsync();
            if(permissonEntity == null)
            {
                throw new FriendlyException(StatusCodes.Status404NotFound, "Not found permission");
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                var permissionExisted = await _permissionRepository.Get(s => s.Name.Contains(request.Name)).FirstOrDefaultAsync();
                if (permissionExisted == null)
                {
                    throw new FriendlyException(StatusCodes.Status400BadRequest, "Permission already existed");
                }

                permissonEntity.Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                permissonEntity.Description = request.Description;
            }

            _permissionRepository.Update(permissonEntity);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<PermissionDTO>(permissonEntity);
        }

        public async Task<List<PermissionDTO>> AssignUserPermissionsAsync(List<Guid> PermissionIds, int UserId)
        {
            var permissions = await _permissionRepository.Get(s => PermissionIds.Any(p => s.GUID == p)).ToListAsync();
            
            _userPermissionRepository.DeleteWhere(s => s.UserId == UserId, true);

            _userPermissionRepository.AddRange(permissions.Select(s => new UserPermissionEntity()
            {
                UserId = UserId,
                PermissionId = s.PermissionId
            }).ToList());

            await _unitOfWork.CommitAsync();

            return _mapper.Map<List<PermissionDTO>>(permissions);
        }

        public async Task<bool> DeletePermissionsAsync(Guid guid)
        {
            var permissionEntity = await _permissionRepository.Get(s => s.GUID == guid).FirstOrDefaultAsync();    
            if (permissionEntity == null)
            {
                return false;
            }

            _permissionRepository.DeleteWhere(s => s.GUID == guid);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
