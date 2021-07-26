using JBHiFiChallengeWebAPI.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI.ServiceContracts
{
    public class RateLimitCheckServiceViaDb : IRateLimitCheckService
    {
        IRateLimitService rateLimitService;

        public RateLimitCheckServiceViaDb(IRateLimitService _rateLimitService)
        {
            this.rateLimitService = _rateLimitService;
        }

        // this is an example of using a real db
        // to perform a "begin transaction" to ensure synchronization across the threads
        public bool CheckRateLimit(string keyName)
        {
            bool rateLimitOk;
            using (AppDbContext db = new AppDbContext())
            {
                using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    IDbSet<KeyUsage> keyUsageDbSet = db.KeyUsages;

                    rateLimitOk = rateLimitService.AddKeyUsage(keyUsageDbSet, keyName);

                    if (rateLimitOk)
                    {
                        tran.Commit();
                        db.SaveChanges();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
            }

            return rateLimitOk;
        }
    }
}
