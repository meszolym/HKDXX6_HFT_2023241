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
    public class Case
    {

        //ID of the case
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        //Name of the case
        [Required]
        [StringLength(240)]
        public string Name { get; set; }

        //Short description of the case
        [Required]
        public string Description { get; set; }

        //Opening timestamp
        [Required]
        public DateTime OpenedAt { get; set; }

        //Closing timestamp
        public DateTime? ClosedAt { get; set; }

        public bool IsClosed
        {
            get { return ClosedAt != null; }
        }


        // Lazyload the officers on the case
        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<Officer> Officers { get; set; }

        //The badge number of the primary officer
        [Range(1000, 99999)]
        public int? PrimaryOfficerBadgeNo { get; set; }

        //Lazyload the primary officer
        public virtual Officer? PrimaryOfficer { get; set; }

        public Precinct? Precinct
        {
            get
            {
                return PrimaryOfficer.Precinct;
            }
        }

        public Case()
        {
            Officers = new HashSet<Officer>();
        }

        public Case(int iD, string name, string description, DateTime openDT)
        {
            ID = iD;
            Name = name;
            Description = description;
            Officers = new HashSet<Officer>();
            OpenedAt = openDT;
        }

        public Case(int iD, string name, string description, int primaryOfficerBadgeNo, DateTime openDT)
        {
            ID = iD;
            Name = name;
            Description = description;
            Officers = new HashSet<Officer>();
            PrimaryOfficerBadgeNo = primaryOfficerBadgeNo;
            OpenedAt = openDT;
        }

    }
}
