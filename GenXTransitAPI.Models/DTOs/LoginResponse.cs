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
        public int RoleId { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public List<LoginPermissionResponse> Permissions { get; set; }
       = new List<LoginPermissionResponse>();

        // public bool IsFirstLogin { get; set; }

        // public string Message { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    public class LoginPermissionResponse
    {
        public int MenuId { get; set; }
        public string? IconName { get; set; }

        public int? SortOrder { get; set; }

        public string? MenuName { get; set; }

        public bool CanView { get; set; }

        public bool CanAdd { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }
    }
}
