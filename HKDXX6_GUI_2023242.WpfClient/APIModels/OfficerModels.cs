using CommunityToolkit.Mvvm.ComponentModel;
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
    public class MinimalOfficerModel : ObservableObject
    {
        [JsonIgnore]
        string firstName;

        public string FirstName 
        {
            get { return firstName; }
            set { SetProperty(ref firstName, value); }
        }

        [JsonIgnore]
        string lastName;

        public string LastName
        {
            get { return lastName; }
            set { SetProperty(ref lastName, value);}
        }

        [JsonIgnore]
        Ranks rank;

        public Ranks Rank 
        {
            get { return rank; }
            set { SetProperty(ref rank, value);}
        }

        [JsonIgnore]
        protected int? directCO_BadgeNo;
        public int? DirectCO_BadgeNo
        {
            get { return directCO_BadgeNo; }
            set { SetProperty(ref directCO_BadgeNo, value);}
        }

        [JsonIgnore]
        protected int precinctID;

        public int PrecinctID
        {
            get { return precinctID; }
            set { SetProperty(ref precinctID, value); }
        }

        [JsonIgnore]
        DateTime hireDate;
        public DateTime HireDate
        {
            get { return hireDate; }
            set { SetProperty( ref hireDate, value); }
        }

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
        [JsonIgnore]
        int badgeNo;
        public int BadgeNo 
        {
            get { return badgeNo; }
            set { SetProperty(ref badgeNo, value); }
        }


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
                SetProperty(ref directCO, value);
                if (value != null)
                {
                    SetProperty(ref directCO_BadgeNo, value.badgeNo);
                }
                else
                {
                    SetProperty(ref directCO_BadgeNo, null);
                }
                
            } 
        }

        [JsonProperty(nameof(DirectCO))]
        public FullOfficerModel SetDirectCO
        {
            set
            {
                SetProperty(ref directCO, value);
            }
        }

        [JsonIgnore]
        PrecinctModel precint;

        [JsonIgnore]
        public PrecinctModel Precinct
        {
            get { return precint; }
            set 
            {
                SetProperty(ref precint, value);
                if (value != null)
                {
                    SetProperty(ref precinctID, value.ID);
                }
            }
        }

        [JsonProperty(nameof(Precinct))]
        public PrecinctModel SetPrecinct
        {
            set
            {
                SetProperty(ref precint, value);
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
