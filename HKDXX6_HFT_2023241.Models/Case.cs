using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Models
{
    public class Case
    {
        

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint ID { get; set; }

        [Required]
        [StringLength(240)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public Case() { }

        public Case(uint iD, string name, string description)
        {
            ID = iD;
            Name = name;
            Description = description;
        }
    }
}
