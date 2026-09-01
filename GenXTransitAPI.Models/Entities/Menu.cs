using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.Models.Entities
{
    public class Menu
    {
         
        public int Id { get; set; }

        public string? IconName { get; set; }

        public int SectionId { get; set; }

        public int? SortOrder { get; set; }

        public string MenuName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}

