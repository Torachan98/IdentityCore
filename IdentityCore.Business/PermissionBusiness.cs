using System.Linq;
using AutoMapper;
using IdentityCore.Business.Interfaces;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using IdentityCore.Repository.Interfaces;
using IdentityCore.Repository.UnitOfWork;
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

        public Task<PermissionDTO> CreatePermissionsAsync(CreateOrUpdatePermissionRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PermissionDTO> UpdatePermissionsAsync(CreateOrUpdatePermissionRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePermissionsAsync(string guid)
        {
            throw new NotImplementedException();
        }
    }
}
