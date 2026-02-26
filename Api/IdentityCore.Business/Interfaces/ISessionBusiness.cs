using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Entities;

namespace IdentityCore.Business.Interfaces
{
    public interface ISessionBusiness
    {
        Task<List<SessionEntity>> GetSessionsByUserAsync(int UserId);
        Task<SessionEntity> CreateSession(SessionEntity sessionEntity);
        Task<SessionEntity> UpdateSession(UserDTO userDto,SessionEntity sessionEntity);
        void DeleteSession(Guid guid,bool isPhysical = false);
    }
}
