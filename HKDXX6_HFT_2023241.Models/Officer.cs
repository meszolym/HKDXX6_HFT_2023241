using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Models
{
    public enum Ranks
    {
        Recruit = 1,
        PatrolOfficer = 2,
        Detective = 3,
        Sergeant = 4,
        Lieutenant = 5,
        Captain = 6

    }
    public class Officer
    {
        [Key]
        [Range(1000,9999)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint BadgeNo { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string Name
        { 
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Required]
        [Range(1,6)]
        public Ranks Rank { get; set; }

        //BadgeNo of officer directly "above" the given officer (Direct Commanding Officer), if any
        [Range(1000, 9999)]
        public uint? DirectCO_BadgeNo { get; set; }

        [Required]
        [Range(1, 139)]
        public uint PrecinctID { get; set; }

        public Officer() { }

        public Officer(uint BadgeNo, string FirstName, string LastName, Ranks Rank, uint? DirectCO_BadgeNo = null, uint PrecintID)
        {
            this.BadgeNo = BadgeNo;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Rank = Rank;
            this.DirectCO_BadgeNo = DirectCO_BadgeNo;
            this.PrecinctID = PrecintID;
        }

    }
}
