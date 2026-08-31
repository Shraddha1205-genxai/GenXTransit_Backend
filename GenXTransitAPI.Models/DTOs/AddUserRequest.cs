using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.Models.DTOs
{
    public class AddUserRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        public string MobileNo { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }
        // public int RoleId { get; set; }
    }

    public class AddUserResponse
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

    public class DeleteUserRequest
    {
        public int UserId { get; set; }
    }

    //public class UserResponse
    //{
    //    public int UserId { get; set; }

    //    public string UserName { get; set; }

    //    public string Email { get; set; }

    //    public string MobileNo { get; set; }
    //    public string PasswordHash { get; set; } = string.Empty;

    //    public string FirstName { get; set; }

    //    public string LastName { get; set; }

    //    public int RoleId { get; set; }

    //    public string RoleName { get; set; }

    //    public bool IsActive { get; set; }

    //    public DateTime CreatedDate { get; set; }

    //    public DateTime? UpdatedDate { get; set; }
    //}

}
