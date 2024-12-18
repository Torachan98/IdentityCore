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
    public class ServiceBusiness : IServiceBusiness
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceRepository _serviceRepository;

        public ServiceBusiness(IMapper mapper, IUnitOfWork unitOfWork, IServiceRepository serviceRepository) 
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _serviceRepository = serviceRepository;
        }

        public async Task<PaginationItems<ServiceDTO>> GetServices(ServiceFetchRequest request)
        {
            var serviceQuery = _serviceRepository.Get(s => !s.IsDeleted);
            int pageNum = 0;
            int pageSize = 30;
            int totalCount = serviceQuery.Count(s => !s.IsDeleted);

            if (!string.IsNullOrEmpty(request.PageNum) && !string.IsNullOrEmpty(request.PageSize))
            {
                pageNum = int.Parse(request.PageNum);
                pageSize = int.Parse(request.PageSize);

                serviceQuery = serviceQuery.Skip(pageNum * (pageSize - 1)).Take(pageSize);
            }

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                serviceQuery = serviceQuery.Where(s => s.Name.Contains(request.Keyword));
            }

            var serviceEntities = await serviceQuery.ToListAsync();

            return new PaginationItems<ServiceDTO>(pageNum, pageSize, totalCount, _mapper.Map<List<ServiceDTO>>(serviceEntities));
        }

        public async Task<ServiceDTO> CreateServicesAsync(CreateOrUpdateServiceRequest request)
        {
            if (request == null || (string.IsNullOrEmpty(request.Name) && string.IsNullOrEmpty(request.SignatureKey)))
            {
                return new ServiceDTO();
            }

            var isExistedService = await _serviceRepository.Get().FirstOrDefaultAsync(s => s.Name.Contains(request.Name));
            if (isExistedService != null)
            {
                if (!isExistedService.IsDeleted)
                {
                    isExistedService.IsDeleted = false;
                    isExistedService.DateModified = DateTime.UtcNow;
                    _serviceRepository.Update(isExistedService, s => s.IsDeleted);
                    await _unitOfWork.CommitAsync();
                    return _mapper.Map<ServiceDTO>(isExistedService);
                }
            }

            var serviceEntities = new ServiceEntity()
            {
                Name = request.Name,
                SignatureKey = request.SignatureKey,
                Description = request.Description,
            };

            var result = _serviceRepository.Add(serviceEntities);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ServiceDTO>(result);
        }

        public async Task<ServiceDTO> UpdateServicesAsync(CreateOrUpdateServiceRequest request)
        {
            var isExistedService = await _serviceRepository.Get().FirstOrDefaultAsync(s => s.GUID == request.GUID);
            if (isExistedService == null)
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Not found service");
            }

            var isExistedServiceName = await _serviceRepository.Get().FirstOrDefaultAsync(s => s.Name == request.Name);
            if (isExistedServiceName != null)
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Name is already existed");
            }

            isExistedService.Description = request.Description;
            isExistedService.DateModified = DateTime.UtcNow;
            return _mapper.Map<ServiceDTO>(isExistedServiceName);
        }

        public async Task<bool> DeleteServicesAsync(string guid)
        {
            var isExistedService = await _serviceRepository.Get().FirstOrDefaultAsync(s => s.GUID == guid);
            if (isExistedService == null)
            {
                return false;
            }

            _serviceRepository.DeleteWhere(s => s.GUID == guid);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
