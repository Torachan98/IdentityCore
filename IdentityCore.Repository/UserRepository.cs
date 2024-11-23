using IdentityCore.EFs.Entities;
using IdentityCore.Repository.Base;
using IdentityCore.Repository.Interfaces;
using IdentityCore.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Repository
{
    public class UserRepository : Repository<UserEntity>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
