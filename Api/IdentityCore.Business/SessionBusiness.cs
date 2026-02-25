using IdentityCore.Business.Interfaces;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Entities;
using IdentityCore.EFs.Requests;
using IdentityCore.Repository;
using IdentityCore.Repository.Interfaces;
using IdentityCore.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Business
{
    public class SessionBusiness : ISessionBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;

        public SessionBusiness(IUnitOfWork unitOfWork,ISessionRepository sessionRepository, IUserRepository userRepository) 
        {
            _unitOfWork = unitOfWork;
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }

        public async Task<SessionEntity> CreateSession(SessionEntity sessionEntity)
        {
            if(sessionEntity == null)
            {
                throw new Exception("Session not found");
            }

            var sessionExisted = await _sessionRepository.Get(s => 
                                        s.RefreshToken == sessionEntity.RefreshToken && 
                                        s.UserId == sessionEntity.UserId && 
                                        !s.IsDeleted).FirstOrDefaultAsync();

            if (sessionExisted != null)
            {
                return sessionExisted;
            }

            _sessionRepository.Add(sessionEntity);
            await _unitOfWork.CommitAsync();
            return sessionEntity;
        }

        public async Task<SessionEntity> UpdateSession(UserDTO userDto,SessionEntity sessionEntity)
        {
            var userEntity = await _userRepository.Get(s=> s.GUID == userDto.GUID).FirstOrDefaultAsync();
            
            if(userEntity == null)
            {
                throw new Exception("User not found");
            }

            var findSession = await _sessionRepository.Get(s => s.DeviceID == sessionEntity.DeviceID && s.UserId == userEntity.UserId).FirstOrDefaultAsync();

            if (findSession == null)
            {
                throw new Exception("Session not found");
            }

            findSession.ExpiredDate = DateTime.UtcNow;
            findSession.IsDeleted = sessionEntity.IsDeleted;
            _sessionRepository.Update(findSession);
            await _unitOfWork.CommitAsync();

            return findSession;
        }

        public void DeleteSession(Guid guid, bool isPhysical = false)
        {
            _sessionRepository.DeleteWhere(s => s.GUID == guid, isPhysical);
            _unitOfWork.Commit();
        }
    }
}
