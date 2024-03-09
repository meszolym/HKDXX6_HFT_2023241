using HKDXX6_HFT_2023241.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Models.NonCrudModels
{
    public class OfficerCaseAverageOpenTimeItem
    {
        public Officer officer { get; set; }
        public string openTimeSpanString { get; set; }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public TimeSpan openTimeSpan {
            get
            {
                return TimeSpan.Parse(this.openTimeSpanString);
            }
        }

        public OfficerCaseAverageOpenTimeItem() { }

        public OfficerCaseAverageOpenTimeItem(Officer officer, TimeSpan openTimeSpan)
        {
            this.officer = officer;
            this.openTimeSpanString = openTimeSpan.ToString();
        }

        public override bool Equals(object obj)
        {
            OfficerCaseAverageOpenTimeItem b = obj as OfficerCaseAverageOpenTimeItem;
            if (b == null)
            {
                return false;
            }
            else
            {
                return
                    officer.Equals(b.officer)
                    && openTimeSpanString.Equals(b.openTimeSpanString);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(officer,openTimeSpanString);
        }
    }
}
