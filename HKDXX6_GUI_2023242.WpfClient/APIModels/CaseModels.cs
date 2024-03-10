using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HKDXX6_GUI_2023242.WpfClient.APIModels
{
    public class MinimalCaseModel : ObservableObject
    {
        [JsonIgnore]
        string name;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        [JsonIgnore]
        string description;
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        [JsonIgnore]
        DateTime openedAt;

        public DateTime OpenedAt
        {
            get
            {
                return openedAt;
            }
            set { SetProperty(ref openedAt, value); }
        }

        [JsonIgnore]
        DateTime? closedAt;
        public DateTime? ClosedAt 
        {
            get
            {
                return closedAt;
            }
            set
            {
                SetProperty(ref closedAt, value);
            }
        }

        [JsonIgnore]
        int? officerOnCaseID;

        public int? OfficerOnCaseID 
        {
            get
            {
                return officerOnCaseID;
            }
            set
            {
                SetProperty(ref officerOnCaseID, value);
            }
        }

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
        [JsonIgnore]
        int iD;
        public int ID
        {
            get
            {
                return iD;
            }
            set
            {
                SetProperty(ref iD, value);
            }
        }

        [JsonIgnore]
        DateTime? closedAt;

        public new DateTime? ClosedAt
        {
            get
            {
                return closedAt;
            }
            set
            {
                SetProperty(ref closedAt, value);
                OnPropertyChanged(nameof(isClosed));
                OnPropertyChanged(nameof(IsClosed));
                OnPropertyChanged(nameof(openTimeSpan));
                OnPropertyChanged(nameof(OpenTimeSpan));
            }
        }

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
                SetProperty(ref isClosed, value);
            }
        }

        [JsonProperty(nameof(IsClosed))]
        public bool SetIsClosed 
        { 
            set 
            {
                SetProperty(ref isClosed, value);
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
                SetProperty(ref openTimeSpan, value);
            }
        }

        [JsonProperty(nameof(OpenTimeSpan))]
        public TimeSpan? SetOpenTimeSpan
        {
            set
            {
                SetProperty(ref openTimeSpan, value);
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
                SetProperty(ref officerOnCase, value);
                if (officerOnCase != null)
                {
                    OfficerOnCaseID = value.BadgeNo;
                }
                
            }
        }

        [JsonProperty(nameof(OfficerOnCase))]
        public FullOfficerModel SetOfficerOnCase
        {
            set
            {
                SetProperty(ref officerOnCase, value);
                if (officerOnCase != null)
                {
                    OfficerOnCaseID = value.BadgeNo;
                }
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
