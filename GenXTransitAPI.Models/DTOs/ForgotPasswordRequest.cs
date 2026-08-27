using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.Models.DTOs
{
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
    }

    public class PasswordResetTokenResult
    {
        public int RowsAffected { get; set; }

        public string? Message { get; set; }
    }

    public class ResetPasswordRequest
    {
        //public int Id { get; set; }
        public string Token { get; set; }
        [Required (ErrorMessage = "New password is required")]

        public string NewPassword { get; set; } = string.Empty;
        [Required (ErrorMessage = "Compaire password is required") ]
        [Compare("NewPassword", ErrorMessage = "New password and confirm password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class ResetPasswordResult
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public int? UserId { get; set; }
    }
}
