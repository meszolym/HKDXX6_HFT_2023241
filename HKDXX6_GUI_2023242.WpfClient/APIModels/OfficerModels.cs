using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_GUI_2023242.WpfClient.APIModels
{
    public enum Ranks
    {
        Recruit = 1,
        Officer = 2,
        Detective = 3,
        Sergeant = 4,
        Lieutenant = 5,
        Captain = 6

    }
    public class MinimalOfficerModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Ranks Rank { get; set; }
        public int? DirectCO_BadgeNo { get; set; }
        public int PrecinctID { get; set; }
        public DateTime HireDate { get; set; }

        public override bool Equals(object obj)
        {
            MinimalOfficerModel b = obj as MinimalOfficerModel;
            if (b == null)
            {
                return false;
            }
            return FirstName == b.FirstName
                    && LastName == b.LastName
                    && Rank == b.Rank
                    && DirectCO_BadgeNo == b.DirectCO_BadgeNo
                    && PrecinctID == b.PrecinctID
                    && HireDate == b.HireDate;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(FirstName);
            hash.Add(LastName);
            hash.Add(Rank);
            hash.Add(DirectCO_BadgeNo);
            hash.Add(PrecinctID);
            hash.Add(HireDate);

            return hash.ToHashCode();
        }

    }

    public class FullOfficerModel : MinimalOfficerModel
    {
        public int BadgeNo { get; set; }


        [JsonIgnore]
        public string FullNameWithRankAndBadgeNo
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Rank.ToString());
                sb.Append(" ");
                sb.Append(FirstName);
                sb.Append(" ");
                sb.Append(LastName);
                sb.Append(" (");
                sb.Append(BadgeNo);
                sb.Append(")");
                return sb.ToString();
            }
        }

        public override string ToString()
        {
            return FullNameWithRankAndBadgeNo;
        }

        [JsonIgnore]
        FullOfficerModel directCO;

        [JsonIgnore]
        public FullOfficerModel DirectCO { 
            get
            { 
                return directCO;
            } 
            set 
            { 
                directCO = value;
            } 
        }

        [JsonProperty(nameof(DirectCO))]
        public FullOfficerModel SetDirectCO
        {
            set
            {
                directCO = value;
            }
        }

        [JsonIgnore]
        PrecinctModel precint;

        [JsonIgnore]
        public PrecinctModel Precinct { get; set; }

        [JsonProperty(nameof(Precinct))]
        public PrecinctModel SetPrecinct
        {
            set
            {
                precint = value;
            }
        }

        public override bool Equals(object obj)
        {
            FullOfficerModel b = obj as FullOfficerModel;
            if (b == null)
            {
                return false;
            }
            return BadgeNo == b.BadgeNo
                    && FirstName == b.FirstName
                    && LastName == b.LastName
                    && Rank == b.Rank
                    && DirectCO_BadgeNo == b.DirectCO_BadgeNo
                    && PrecinctID == b.PrecinctID
                    && HireDate == b.HireDate;
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
