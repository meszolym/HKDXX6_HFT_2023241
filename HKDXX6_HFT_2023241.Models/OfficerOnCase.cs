using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Models
{
    public class OfficerOnCase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint ID { get; set; }

        [Required]
        public uint CaseID { get; set; }

        [Required]
        [Range(1000, 9999)]
        public uint OfficerBadgeNo { get; set; }

        [Required]
        public bool IsPrimary { get; set; }

        public OfficerOnCase() { }
        public OfficerOnCase(uint id, uint caseID, uint officerBadgeNo, bool isPrimary)
        {
            ID = id;
            CaseID = caseID;
            OfficerBadgeNo = officerBadgeNo;
            IsPrimary = isPrimary;
        }
    }
}
