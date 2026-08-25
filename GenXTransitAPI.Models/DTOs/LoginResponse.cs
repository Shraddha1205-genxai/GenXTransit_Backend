using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.Models.DTOs
{
    public class LoginResponse
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

       // public int RoleId { get; set; }

        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

       // public bool IsFirstLogin { get; set; }

       // public string Message { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string LoginId { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
