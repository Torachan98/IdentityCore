using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.EFs.Requests
{
    public class FetchParams
    {
        public string? Keyword { get; set; }
        public string? PageSize { get; set; }
        public string? PageNum { get; set; }       
    }
}
