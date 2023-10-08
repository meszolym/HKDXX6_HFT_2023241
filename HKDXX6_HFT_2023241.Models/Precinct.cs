using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HKDXX6_HFT_2023241.Models
{
    public class Precinct
    {
        [Key]
        [Range(1,139)]
        public uint ID { get; set; }

        //Badge number of the officer in charge of the precinct
        [Required]
        [Range(1000, 9999)]
        public uint COBadgeNo { get; set; }

        [Required]
        [StringLength(100)]
        public string Address;

        [NotMapped]
        [JsonIgnore]
        public string Area
        {
            get
            {
                switch (ID)
                {
                    case >= 1 and <= 39:
                        return "Manhattan";
                    case >= 40 and <= 59:
                        return "Bronx";
                    case >= 60 and <= 99:
                        return "Brooklyn";
                    case >= 100 and <= 119:
                        return "Queens";
                    case >= 120 and <= 139:
                        return "Staten Island";
                    default:
                        return "N/A";
                }

            }
        }

        public Precinct() { }
        public Precinct(uint iD, uint cOBadgeNo, string address)
        {
            ID = iD;
            COBadgeNo = cOBadgeNo;
            Address = address;
        }
    }
}
