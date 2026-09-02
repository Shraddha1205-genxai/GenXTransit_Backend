using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.Models.DTOs
{
    public class AuthorizationRowDto
    {
        public int AuthId { get; set; }

        public int RoleId { get; set; }

        public int SectionId { get; set; }
        public string? SectionName { get; set; }

        public int MenuId { get; set; }
        public string? MenuName { get; set; }

        public int TabId { get; set; }
        public string? TabName { get; set; }

        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }

        public bool IsDisableView { get; set; }

        public bool IsDisableEdit { get; set; }

        public bool IsDisableAdd { get; set; }

        public bool IsDisableDelete { get; set; }

        public bool CanAction { get; set; }

        public bool IsDefault { get; set; }
    }
}

    public class AuthorizationItem
    {
        public int? AuthId { get; set; }

        public int RoleId { get; set; }

        public int SectionId { get; set; }

        public int MenuId { get; set; }

        public int TabId { get; set; }

        public bool CanView { get; set; }

        public bool CanAdd { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        //public bool IsDisableView { get; set; }

        //public bool IsDisableEdit { get; set; }

        //public bool IsDisableAdd { get; set; }

        //public bool IsDisableDelete { get; set; }

        public bool CanAction { get; set; }

        public bool IsDefault { get; set; }
    }

    //public class AuthorizationSaveDto
    //{
    //    public int RoleId { get; set; }
    //    public int SectionId { get; set; }
    //    public int MenuId { get; set; }
    //    public int TabId { get; set; }

    //    public bool CanView { get; set; }
    //    public bool CanAdd { get; set; }
    //    public bool CanEdit { get; set; }
    //    public bool CanDelete { get; set; }

    //    //public bool IsDisableView { get; set; }
    //    //public bool IsDisableEdit { get; set; }
    //    //public bool IsDisableAdd { get; set; }
    //    //public bool IsDisableDelete { get; set; }

    //    public bool CanAction { get; set; }
    //    public bool IsDisableAction { get; set; }
    //}

    //public class AuthorizationUpdateDto
    //{
    //    public int AuthId { get; set; }

    //    public int RoleId { get; set; }
    //    public int SectionId { get; set; }
    //    public int MenuId { get; set; }
    //    public int TabId { get; set; }

    //    public bool CanView { get; set; }
    //    public bool CanAdd { get; set; }
    //    public bool CanEdit { get; set; }
    //    public bool CanDelete { get; set; }

    //    //public bool IsDisableView { get; set; }
    //    //public bool IsDisableEdit { get; set; }
    //    //public bool IsDisableAdd { get; set; }
    //    //public bool IsDisableDelete { get; set; }

    //    public bool CanAction { get; set; }
    //    public bool IsDefault { get; set; }
    //}

