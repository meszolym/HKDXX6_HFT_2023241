using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Models
{
    public class OfficerOnCase
    {
        [Required]
        public uint CaseID { get; set; }

        [Required]
        [Range(1000, 9999)]
        public uint OfficerBadgeNo { get; set; }

        [Required]
        public bool IsPrimary { get; set; }
    }
}
