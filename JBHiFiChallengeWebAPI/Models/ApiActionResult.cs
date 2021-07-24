using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI.Models
{
    public class ApiActionResult
    {
        public object Data { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }

        public ApiActionResult(object data, string message = null, bool isSuccess = true)
        {
            this.Data = data;
            this.Message = message;
            this.IsSuccess = isSuccess;
        }
    }
}
