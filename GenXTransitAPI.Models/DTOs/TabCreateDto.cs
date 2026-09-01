using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.Models.DTOs
{
    public class TabCreateDto
    {
        public int SectionId { get; set; }
        public int MenuId { get; set; }
        public string? TabName { get; set; }
        public int? SortOrder { get; set; }
        public string? URL { get; set; }
        public bool IsActive { get; set; }
        //public int CreatedBy { get; set; }
    }

    public class TabUpdateDto
    {
        public int TabId { get; set; }
        public int SectionId { get; set; }
        public int MenuId { get; set; }
        public string? TabName { get; set; }
        public int? SortOrder { get; set; }
        public string? URL { get; set; }
        public bool IsActive { get; set; }
       // public int ModifiedBy { get; set; }
    }
    public class DeleteTabRequest
    {
        public int TabId { get; set; }
        //public int ModifiedBy { get; set; }
    }
}
