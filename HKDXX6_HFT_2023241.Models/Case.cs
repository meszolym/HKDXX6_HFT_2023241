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
        public uint ID { get; set; }

        //Name of the case
        [Required]
        [StringLength(240)]
        public string Name { get; set; }

        //Short description of the case
        [Required]
        public string Description { get; set; }

        // Lazyload the officers on the case
        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<Officer> Officers { get; set; }

        public Case()
        {
            Officers = new HashSet<Officer>();
        }

        public Case(uint iD, string name, string description)
        {
            ID = iD;
            Name = name;
            Description = description;
            Officers = new HashSet<Officer>();
        }


    }
}
