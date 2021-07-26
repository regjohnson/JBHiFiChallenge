using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI.ServiceContracts
{
    public interface IRateLimitCheckService
    {
        public bool CheckRateLimit(string keyName);
    }
}
