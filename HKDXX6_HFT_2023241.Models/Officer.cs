using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;

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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BadgeNo { get; set; }

        //The first name of the officer
        [Required]
        [StringLength(100)]
        [MinLength(2)]
        public string FirstName { get; set; }

        //The last name of the officer
        [Required]
        [StringLength(100)]
        [MinLength(2)]
        public string LastName { get; set; }

        //The rank of the officer
        [Required]
        [Range(1, 6)]
        public Ranks Rank { get; set; }

        //BadgeNo of officer directly "above" the officer (Direct Commanding Officer), if any
        public int? DirectCO_BadgeNo { get; set; }

        //Lazyload the DCO of the officer
        [NotMapped]
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
        public virtual Precinct Precinct { get; set; }

        //Lazyload all cases where the officer is attached
        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<Case> Cases { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        public Officer()
        {
            Cases = new HashSet<Case>();
            OfficersUnderCommand = new HashSet<Officer>();
        }

        public Officer(int BadgeNo, string FirstName, string LastName, Ranks Rank, int? DirectCO_BadgeNo, int PrecintID, DateTime hireDate)
        {
            this.BadgeNo = BadgeNo;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Rank = Rank;
            this.DirectCO_BadgeNo = DirectCO_BadgeNo;
            this.PrecinctID = PrecintID;
            Cases = new HashSet<Case>();
            OfficersUnderCommand = new HashSet<Officer>();
            HireDate = hireDate;
        }

        public override bool Equals(object obj)
        {
            Officer b = obj as Officer;
            if (b == null)
            {
                return false;
            }
            else
            {
                return BadgeNo == b.BadgeNo
                    && FirstName == b.FirstName
                    && LastName == b.LastName
                    && Rank == b.Rank
                    && DirectCO_BadgeNo == b.DirectCO_BadgeNo
                    && PrecinctID == b.PrecinctID
                    && HireDate == b.HireDate;
            }
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(BadgeNo);
            hash.Add(FirstName);
            hash.Add(LastName);
            hash.Add(Rank);
            hash.Add(DirectCO_BadgeNo);
            hash.Add(PrecinctID);
            hash.Add(HireDate);

            return hash.ToHashCode();
        }
    }
}
