using JBHiFiChallengeWebAPI.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI.ServiceContracts
{
    public interface IRateLimitService
    {
        public bool AddKeyUsage(IDbSet<KeyUsage> keyUsageDbSet, string keyName);
        public bool AddKeyUsage(IDbSet<KeyUsage> keyUsageDbSet, string keyName, DateTime usageUTCDate);
    }
}
