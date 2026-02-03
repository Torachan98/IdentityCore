using AutoMapper;
using IdentityCore.Business.Interfaces;
using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Enums;
using IdentityCore.Repository.Interfaces;
using IdentityCore.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace IdentityCore.Business
{
    public class CronJobBusiness : ICronJobBusiness
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IEmailBusiness _emailBusiness;

        private readonly IUserRepository _userRepository;

        public CronJobBusiness(IDistributedCache distributedCache, 
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IEmailBusiness emailBusiness,
            IUserRepository userRepository) 
        {
            _mapper = mapper;
            _distributedCache = distributedCache;
            _unitOfWork = unitOfWork;
            _emailBusiness = emailBusiness;
            _userRepository = userRepository;
        }

        public async Task CleanBlackListAsync()
        {
            string blackListJson = await _distributedCache.GetStringAsync(KeyCache.BlackList) ?? "[]";
            var blacklist = JsonConvert.DeserializeObject<List<TokenBlacklist>>(blackListJson);

            if(blacklist!.Count == 0)
            {
                return;
            }

            var blacklistRemoveOldToken = blacklist!.Where(s => s.DateExpired.AddMinutes(GlobalConst.Jwt.LifeTime) < DateTime.UtcNow).ToList();
            await _distributedCache.SetStringAsync(KeyCache.BlackList, JsonConvert.SerializeObject(blacklistRemoveOldToken));
        }

        public async Task ResetOTPAsync()
        {
            var userEntities = await _userRepository.Get(s => !s.IsDeleted).Where(s => s.OTPLifeTime!.Value < DateTime.UtcNow).ToListAsync();

            if (userEntities!.Count == 0)
            {
                return;
            }

            foreach (var user in userEntities)
            {
                user.OTPLifeTime = null;
                user.OTPCode = null;
                _userRepository.Update(user);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task UnLockUsersAsync()
        {
            var userEntities = await _userRepository.Get(s => !s.IsDeleted).Where(s => s.Locked!.Value < DateTime.UtcNow).ToListAsync();
            foreach (var user in userEntities)
            {
                user.Locked = null;
                _userRepository.Update(user);
            }

            await _unitOfWork.CommitAsync();

            foreach (var user in userEntities)
            {
                await _emailBusiness.SendMailAsync(_mapper.Map<UserDTO>(user), TemplateEmailType.Unlocked);
            }            
        }
    }
}
