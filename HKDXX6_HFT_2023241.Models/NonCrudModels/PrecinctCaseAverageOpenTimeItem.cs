using HKDXX6_HFT_2023241.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Models.NonCrudModels
{
    public class PrecinctCaseAverageOpenTimeItem
    {
        public Precinct precinct { get; set; }
        public string openTimeSpanString { get; set; }

        [JsonIgnore]
        public TimeSpan openTimeSpan {
            get
            {
                return TimeSpan.Parse(this.openTimeSpanString);
            }
        }

        public PrecinctCaseAverageOpenTimeItem() { }

        public PrecinctCaseAverageOpenTimeItem(Precinct precinct, TimeSpan openTimeSpan)
        {
            this.precinct = precinct;
            this.openTimeSpanString = openTimeSpan.ToString();
        }

        public override bool Equals(object obj)
        {
            PrecinctCaseAverageOpenTimeItem b = obj as PrecinctCaseAverageOpenTimeItem;
            if (b == null)
            {
                return false;
            }
            else
            {
                return
                    precinct.Equals(b.precinct)
                    && openTimeSpanString.Equals(b.openTimeSpanString);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(precinct,openTimeSpanString);
        }
    }
}
