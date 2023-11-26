using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Models.DBModels
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
        [MinLength(10)]
        public string Name { get; set; }

        //Short description of the case
        [Required]
        [MinLength(15)]
        public string Description { get; set; }

        //Opening timestamp
        [Required]
        public DateTime OpenedAt { get; set; }

        //Closing timestamp
        public DateTime? ClosedAt { get; set; }

        [NotMapped]
        public bool IsClosed
        {
            get { return ClosedAt != null; }
        }

        [NotMapped]
        public TimeSpan? OpenTimeSpan
        {
            get
            {
                return ClosedAt - OpenedAt;
            }
        }

        // Lazyload the officer on the case
        [NotMapped]
        public virtual Officer OfficerOnCase { get; set; }

        public int? OfficerOnCaseID { get; set; }

        [NotMapped]
        [JsonIgnore]
        public Precinct Precinct
        {
            get
            {
                return OfficerOnCase?.Precinct;
            }
        }

        public Case()
        {

        }

        public Case(int iD, string name, string description, DateTime openDT)
        {
            ID = iD;
            Name = name;
            Description = description;
            OpenedAt = openDT;
        }

        public Case(int iD, string name, string description, int OfficerBadgeNo, DateTime openDT)
        {
            ID = iD;
            Name = name;
            Description = description;
            OfficerOnCaseID = OfficerBadgeNo;
            OpenedAt = openDT;
        }

        public override bool Equals(object obj)
        {
            Case b = obj as Case;
            if (b == null)
            {
                return false;
            }
            return ID == b.ID
                    && Name == b.Name
                    && Description == b.Description
                    && OpenedAt.Equals(b.OpenedAt)
                    && ClosedAt.Equals(b.ClosedAt)
                    && OfficerOnCaseID == b.OfficerOnCaseID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Name, Description, OpenedAt, ClosedAt, OfficerOnCaseID);
        }

    }
}
