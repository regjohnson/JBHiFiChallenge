using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI.Models
{
    public class ApiErrorResult
    {
        public int ErrorStatusCode;
        public string ErrorMessage;

        public ApiErrorResult(HttpStatusCode errorStatusCode, string errorMessage)
        {
            this.ErrorStatusCode = (int)errorStatusCode;
            this.ErrorMessage = errorMessage;
        }
    }
}
