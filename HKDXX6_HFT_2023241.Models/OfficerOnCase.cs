using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Models
{
    public class OfficerOnCase
    {
        //The ID of the connection
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint ID { get; set; }

        //The ID of the case
        [Required]
        public uint CaseID { get; set; }

        //Lazyload the case
        [NotMapped]
        [JsonIgnore]
        public virtual Case Case { get; set; }

        //The ID of the officer attached to the case
        [Required]
        [Range(1000, 99999)]
        public uint OfficerBadgeNo { get; set; }

        //Lazyload the officer
        [NotMapped]
        [JsonIgnore]
        public virtual Officer Officer { get; set; }

        //Wheter the officer is primary on the case
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
