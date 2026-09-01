using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.Models.DTOs
{
    public class SectionRequestDto
    {
        public string SectionName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        //public int CreatedBy { get; set; }
    }

    public class SectionUpdateRequestDto
    {
        public int SectionId { get; set; }

        public string SectionName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }
    }
    public class DeleteSectionRequest
    {
        public int SectionId { get; set; }
       //public int ModifiedBy { get; set; }
    }
}
