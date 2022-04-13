using Abstraction.Interfaces.Services;
using Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class VwdService : IVwdService
    {
        public Task<VwdResponse> GetAsync(string vwdKey)
        {
            throw new NotImplementedException();
        }
    }
}
