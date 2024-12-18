using System.Linq;
using AutoMapper;
using IdentityCore.Business.Interfaces;
using IdentityCore.EFs;
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

        public PermissionBusiness(IMapper mapper, IUnitOfWork unitOfWork, IPermissionRepository permissionRepository)
        {
             _mapper = mapper;
            _unitOfWork = unitOfWork;
            _permissionRepository = permissionRepository;
        }

        public async Task<PaginationItems<PermissionDTO>> GetPermissionsAsync(PermissionFetchRequest request)
        {
            var permissionQuery = _permissionRepository.Get(s => !s.IsDeleted);
            int pageNum = 0;
            int pageSize = 30;
            int totalCount = permissionQuery.Count(s => !s.IsDeleted);

            if (!string.IsNullOrEmpty(request.PageNum) && !string.IsNullOrEmpty(request.PageSize))
            {
                pageNum = int.Parse(request.PageNum);
                pageSize = int.Parse(request.PageSize);

                permissionQuery = permissionQuery.Skip(pageNum * (pageSize - 1)).Take(pageSize);
            }

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                permissionQuery = permissionQuery.Where(s => s.Name.Contains(request.Keyword));
            }

            if (request.PermissionTypes.Any())
            {
                permissionQuery = permissionQuery.Where(s => request.PermissionTypes.Contains((long)s.PermissionType));
            }

            var permissionEntities = await permissionQuery.ToListAsync();

            return new PaginationItems<PermissionDTO>(pageNum, pageSize, totalCount, _mapper.Map<List<PermissionDTO>>(permissionEntities));
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
                PermissionType = request.PermissionType,
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
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Not found permission");
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

        public async Task<bool> DeletePermissionsAsync(string guid)
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
