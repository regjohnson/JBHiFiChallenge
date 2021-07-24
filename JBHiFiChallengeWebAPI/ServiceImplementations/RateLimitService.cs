using JBHiFiChallengeWebAPI.Entities;
using JBHiFiChallengeWebAPI.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI.ServiceImplementations
{
    public class RateLimitService : IRateLimitService
    {
        private int MaxKeyLimitPerHour { get; } = 5;

        public bool AddKeyUsage(IDbSet<KeyUsage> keyUsageDbSet, string keyName)
        {
            return AddKeyUsage(keyUsageDbSet, keyName, DateTime.UtcNow);
        }

        public bool AddKeyUsage(IDbSet<KeyUsage> keyUsageDbSet, string keyName, DateTime usageUTCDate)
        {
            DateTime hourAgo = DateTime.UtcNow.AddHours(-1);

            int currentKeyAttempts = keyUsageDbSet.Where(i => i.KeyName == keyName && i.UsageUTCDate > hourAgo).Count();
            if (currentKeyAttempts >= this.MaxKeyLimitPerHour)
            {
                return false;
            }

            var keyUsageEntity = new KeyUsage() { KeyUsageId = Guid.NewGuid(), KeyName = keyName, UsageUTCDate = usageUTCDate };
            keyUsageDbSet.Add(keyUsageEntity);

            return true;
        }
    }
}
