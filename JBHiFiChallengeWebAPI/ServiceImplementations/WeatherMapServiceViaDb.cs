using JBHiFiChallengeWebAPI.Entities;
using JBHiFiChallengeWebAPI.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI.ServiceImplementations
{
    public class WeatherMapServiceViaDb : IWeatherMapService
    {
        IRateLimitService rateLimitService;
        public WeatherMapServiceViaDb(IRateLimitService _rateLimitService)
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
                var tran = db.Database.BeginTransaction();
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

            return rateLimitOk;
        }
    }
}
