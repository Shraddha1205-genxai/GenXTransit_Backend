using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? MobileNo { get; set; }

        public string PasswordHash { get; set; } = string.Empty;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int RoleId { get; set; }

        public bool IsActive { get; set; }

        public bool IsEmailVerified { get; set; }

        public bool IsMobileVerified { get; set; }

        public bool IsFirstLogin { get; set; }

        public DateTime? PasswordChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }
    }

    public class UpdateUserRequest
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? MobileNo { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }

}
