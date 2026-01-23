using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;

namespace IdentityCore.Services.Interfaces
{
    public interface IBaseService<ClassDto,ClassRequest,ClassFetchParams> 
        where ClassDto : class
        where ClassRequest : class
        where ClassFetchParams : class
    {
        Task<PaginationItems<ClassDto>> GetAllAsync(ClassFetchParams request);
        Task<ClassDto> GetByIdAsync(Guid guid);
        Task<ClassDto> CreateAsync(ClassRequest request);
        Task<ClassDto> UpdateAsync(ClassRequest request);
        Task<bool> DeleteAsync(Guid guid);
    }
}
