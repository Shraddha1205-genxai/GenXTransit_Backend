using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.Models.DTO_s
{
    public class RegisterUserRequest
    {
        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? MobileNo { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

       // public int RoleId { get; set; }
    }

    public class RegisterUserResponse
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }

    public class UpdateUserResponse
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? MobileNo { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
