using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HKDXX6_GUI_2023242.WpfClient.APIModels
{
    public class MinimalCaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public int? OfficerOnCaseID { get; set; }

        public override bool Equals(object obj)
        {
            MinimalCaseModel b = obj as MinimalCaseModel;
            if (b == null)
            {
                return false;
            }
            return Name == b.Name
                    && Description == b.Description
                    && OpenedAt.Equals(b.OpenedAt)
                    && ClosedAt.Equals(b.ClosedAt)
                    && OfficerOnCaseID == b.OfficerOnCaseID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Description, OpenedAt, ClosedAt, OfficerOnCaseID);
        }

    }

    public class FullCaseModel : MinimalCaseModel
    {
        public int ID { get; set; }

        [JsonIgnore]
        bool isClosed;

        [JsonIgnore]
        public bool IsClosed
        {
            get
            {
                return isClosed;
            }
            set
            {
                isClosed = value;
            }
        }

        [JsonProperty(nameof(IsClosed))]
        public bool SetIsClosed 
        { 
            set 
            { 
                isClosed = value; 
            }
        }

        [JsonIgnore]
        TimeSpan? openTimeSpan;

        [JsonIgnore]
        public TimeSpan? OpenTimeSpan 
        { 
            get
            {
                return openTimeSpan;
            }
            set
            {
                openTimeSpan = value;
            }
        }

        [JsonProperty(nameof(OpenTimeSpan))]
        public TimeSpan? SetOpenTimeSpan
        {
            set
            {
                openTimeSpan = value;
            }
        }

        [JsonIgnore]
        FullOfficerModel officerOnCase;

        [JsonIgnore]
        public FullOfficerModel OfficerOnCase {
            get
            {
                return officerOnCase;
            }

            set
            {
                officerOnCase = value;
            }
        }

        [JsonProperty(nameof(OfficerOnCase))]
        public FullOfficerModel SetOfficerOnCase
        {
            set
            {
                officerOnCase = value;
            }
        }

        public override bool Equals(object obj)
        {
            FullCaseModel b = obj as FullCaseModel;
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
