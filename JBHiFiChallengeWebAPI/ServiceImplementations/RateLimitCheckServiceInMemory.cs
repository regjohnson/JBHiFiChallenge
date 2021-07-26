using JBHiFiChallengeWebAPI.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI.ServiceContracts
{
    public class RateLimitCheckServiceInMemory : IRateLimitCheckService
    {
        IRateLimitService rateLimitService;

        public RateLimitCheckServiceInMemory(IRateLimitService _rateLimitService)
        {
            this.rateLimitService = _rateLimitService;
        }

        private static IDbSet<KeyUsage> keyUsageDbSet = null;
        public bool CheckRateLimit(string keyName)
        {
            // at this point we could use an actual database connection,
            // then we could perform a "begin transaction" to ensure synchronization across the threads
            // however for this purpose, I will just an in-memory list to store the rate limits
            // see the file: 'WeatherMapServiceViaDb.cs' for example
            if (keyUsageDbSet == null)
            {
                keyUsageDbSet = Helpers.MockDbSet.CreateMockDbSet(new List<KeyUsage>()).Object;
            }

            bool rateLimitOk = rateLimitService.AddKeyUsage(keyUsageDbSet, keyName);
            return rateLimitOk;
        }
    }
}
