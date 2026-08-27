using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.Models.DTOs
{
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; } = string.Empty;
     public string NewPassword { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class PasswordUpdateResult
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string? Data { get; set; }
    }
}
