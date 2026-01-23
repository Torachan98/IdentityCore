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
        private readonly IUserServiceRepository _userServiceRepository;

        public ServiceBusiness(IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IServiceRepository serviceRepository, 
            IUserServiceRepository userServiceRepository) 
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _serviceRepository = serviceRepository;
            _userServiceRepository = userServiceRepository;
        }

        public async Task<PaginationItems<ServiceDTO>> GetServices(ServiceFetchRequest request)
        {
            var serviceQuery = _serviceRepository.Get(s => !s.IsDeleted);
            int pageNum = 1;
            int pageSize = 30;
            int totalCount = serviceQuery.Count(s => !s.IsDeleted);

            if (!string.IsNullOrEmpty(request.PageNum) && !string.IsNullOrEmpty(request.PageSize))
            {
                pageNum = int.Parse(request.PageNum);
                pageSize = int.Parse(request.PageSize);                
            }

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                serviceQuery = serviceQuery.Where(s => s.Name.Contains(request.Keyword));
            }

            serviceQuery = serviceQuery.Skip((pageNum - 1) * pageSize).Take(pageSize);

            var serviceEntities = await serviceQuery.ToListAsync();

            return new PaginationItems<ServiceDTO>(pageSize, pageNum, totalCount, _mapper.Map<List<ServiceDTO>>(serviceEntities));
        }

        public async Task<ServiceDTO> CreateServicesAsync(CreateOrUpdateServiceRequest request)
        {
            if (request == null || (string.IsNullOrEmpty(request.Name) && string.IsNullOrEmpty(request.SignatureKey)))
            {
                return new ServiceDTO();
            }

            var isExistedService = await _serviceRepository.Get(s => !s.IsDeleted).FirstOrDefaultAsync(s => s.Name.Contains(request.Name));
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
            var serviceEntity = await _serviceRepository.Get(s => !s.IsDeleted).FirstOrDefaultAsync(s => s.GUID == request.GUID || s.Name == request.Name);
            if (serviceEntity == null)
            {
                throw new FriendlyException(StatusCodes.Status404NotFound, "Not found service");
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                serviceEntity.Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                serviceEntity.Description = request.Description;
            }

            if (!string.IsNullOrEmpty(request.SignatureKey))
            {
                serviceEntity.SignatureKey = request.SignatureKey;
            }

            _serviceRepository.Update(serviceEntity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ServiceDTO>(serviceEntity);
        }

        public async Task<bool> DeleteServicesAsync(Guid guid)
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

        public async Task<List<AssigningService>> AssigningUserServiceAsync(List<AssigningService> request, int userId)
        {
            _userServiceRepository.DeleteWhere(s => s.UserId == userId, true);

            var servicesRequestId = request.Select(s => s.GUID).ToList();
            var services = await _serviceRepository.Get(s => servicesRequestId.Any(r => s.GUID == r)).ToListAsync();

            var userServiceEntities = _userServiceRepository.AddRange(services.Select(s =>
            {
                var assigningService = request.First(r => r.GUID == s.GUID);
                return new UserServiceEntity()
                {
                    UserId = userId,
                    ServiceId = s.ServiceId,
                    DateActive = assigningService.DateActive.HasValue ? assigningService.DateActive.Value : null,
                    DateExpired = assigningService.DateExpired.HasValue ? assigningService.DateExpired.Value : null
                };
            }).ToList());

            await _unitOfWork.CommitAsync();

            var result = (from b in userServiceEntities
                           join c in services on b.ServiceId equals c.ServiceId
                          where b.UserId == userId
                          group b by c.ServiceId into g
                          select new
                          {
                              Key = g.Key,
                              Items = g.ToList(),
                          })
                          .Select(b => new AssigningService()
                          {
                              Name = services.First(s => s.ServiceId == b.Key).Name,
                              GUID = services.First(s => s.ServiceId == b.Key).GUID,
                              Key = services.First(s => s.ServiceId == b.Key).SignatureKey,
                              DateExpired = b.Items.First(s => s.ServiceId == b.Key).DateExpired,
                              DateActive = b.Items.First(s => s.ServiceId == b.Key).DateActive,
                          }).ToList();


            return result;
        }
    }
}
