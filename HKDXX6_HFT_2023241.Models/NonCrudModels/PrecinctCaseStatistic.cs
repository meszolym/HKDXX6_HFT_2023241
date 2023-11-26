using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Models.NonCrudModels
{
    public class PrecinctCaseStatistic
    {
        public Precinct Precinct { get; set; }
        public int ClosedCases { get; set; }
        public int OpenCases { get; set; }

        public override bool Equals(object obj)
        {
            PrecinctCaseStatistic b = obj as PrecinctCaseStatistic;
            if (b == null)
            {
                return false;
            }
            else
            {
                return this.Precinct.Equals(b.Precinct)
                    && ClosedCases == b.ClosedCases
                    && OpenCases == b.OpenCases;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Precinct, ClosedCases, OpenCases);
        }
    }
}
