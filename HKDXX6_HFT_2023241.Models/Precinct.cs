using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HKDXX6_HFT_2023241.Models
{
    public class Precinct
    {
        //ID of the precinct
        [Key]
        [Range(1, 139)]
        public int ID { get; set; }

        //Lazyload the officers of the precinct
        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<Officer> Officers { get; set; }

        //The address of the precinct in NY
        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        public Precinct()
        {
            Officers = new HashSet<Officer>();
        }
        public Precinct(int iD, string address)
        {
            ID = iD;
            Address = address;
            Officers = new HashSet<Officer>();
        }
    }
}
