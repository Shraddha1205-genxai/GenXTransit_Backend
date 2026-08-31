using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GenXTransitAPI.Models.DTOs
{
    public class CreateRoleRequest
    {
        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(100)]
        public string RoleName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        //public bool IsActive { get; set; } = true;
    }

    public class UpdateRoleRequest
    {
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(100)]
        public string RoleName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }

    public class RoleResponse
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }

    public class DeleteRoleRequest
    {
        public int RoleId { get; set; }
    }
}
