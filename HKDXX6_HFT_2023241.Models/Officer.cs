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
        //The badge number (ID) of the officer
        [Key]
        [Range(1000, 99999)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BadgeNo { get; set; }

        //The first name of the officer
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        //The last name of the officer
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        //The rank of the officer
        [Required]
        [Range(1, 6)]
        public Ranks Rank { get; set; }

        //BadgeNo of officer directly "above" the officer (Direct Commanding Officer), if any
        [Range(1000, 99999)]
        public int? DirectCO_BadgeNo { get; set; }

        //Lazyload the DCO of the officer
        [NotMapped]
        [JsonIgnore]
        public virtual Officer? DirectCO { get; set; }

        //Lazyload officers who are under the command of officer (officers where this officer is DCO)
        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<Officer> OfficersUnderCommand { get; set; }

        //The id of the precinct of the officer, where they are working
        [Required]
        [Range(1, 139)]
        public int PrecinctID { get; set; }

        //Lazyload the precinct
        [NotMapped]
        [JsonIgnore]
        public virtual Precinct Precinct { get; set; }

        //Lazyload all cases where the officer is attached
        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<Case> Cases { get; set; }

        //Lazyload cases where the officer is attached as primary
        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<Case> CasesAsPrimary { get; set; }


        public Officer()
        {
            Cases = new HashSet<Case>();
            CasesAsPrimary = new HashSet<Case>();
            OfficersUnderCommand = new HashSet<Officer>();
        }

        public Officer(int BadgeNo, string FirstName, string LastName, Ranks Rank, int? DirectCO_BadgeNo, int PrecintID)
        {
            this.BadgeNo = BadgeNo;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Rank = Rank;
            this.DirectCO_BadgeNo = DirectCO_BadgeNo;
            this.PrecinctID = PrecintID;
            Cases = new HashSet<Case>();
            CasesAsPrimary = new HashSet<Case>();
            OfficersUnderCommand = new HashSet<Officer>();
        }

    }
}
