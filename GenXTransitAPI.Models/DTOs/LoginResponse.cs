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
        public List<LoginPermissionResponse> Permissions { get; set; } = new();


        // public List<LoginPermissionResponse> Permissions { get; set; }
        //= new List<LoginPermissionResponse>();

        // public bool IsFirstLogin { get; set; }

        // public string Message { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    //public class LoginPermissionResponse
    //{
    //    public int MenuId { get; set; }
    //    public string? IconName { get; set; }

    //    public int? SortOrder { get; set; }

    //    public string? MenuName { get; set; }

    //    public bool CanView { get; set; }

    //    public bool CanAdd { get; set; }

    //    public bool CanEdit { get; set; }

    //    public bool CanDelete { get; set; }
    //}

    public class UserPermissionDto
    {
        public int SectionId { get; set; }

        public string? SectionName { get; set; }

        public int MenuId { get; set; }

        public string? IconName { get; set; }

        public int? MenuSortOrder { get; set; }

        public string? MenuName { get; set; }

        public int TabId { get; set; }

        public string? TabName { get; set; }

        public int? TabSortOrder { get; set; }

        public string? URL { get; set; }

        public bool CanView { get; set; }

        public bool CanAdd { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }
    }

    public class LoginPermissionResponse
    {
        public int SectionId { get; set; }

        public string? SectionName { get; set; }

        public List<PermissionMenuResponse> MenuList { get; set; } = new();
    }

    public class PermissionMenuResponse
    {
        public int MenuId { get; set; }

        public string? IconName { get; set; }

        public int? SortOrder { get; set; }

        public string? MenuName { get; set; }

        public List<PermissionTabResponse> TabList { get; set; } = new();
    }

    public class PermissionTabResponse
    {
        public int TabId { get; set; }

        public string? TabName { get; set; }

        public bool CanView { get; set; }

        public bool CanAdd { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public int? SortOrder { get; set; }

        public string? Url { get; set; }
    }
}
