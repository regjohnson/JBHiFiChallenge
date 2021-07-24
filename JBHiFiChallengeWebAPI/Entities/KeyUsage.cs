using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI.Entities
{
    public class KeyUsage
    {
        public Guid KeyUsageId { get; set; }
        public string KeyName { get; set; }
        public DateTime UsageUTCDate { get; set; }
    }
}
