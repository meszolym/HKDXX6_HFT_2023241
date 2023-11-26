using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Models.NonCrudModels
{
    public class OfficerCaseStatistic
    {
        public Officer Officer { get; set; }
        public int ClosedCases { get; set; }
        public int OpenCases { get; set; }

        public override bool Equals(object obj)
        {
            OfficerCaseStatistic b = obj as OfficerCaseStatistic;
            if (b == null)
            {
                return false;
            }
            else
            {
                return
                    Officer.Equals(b.Officer)
                    && ClosedCases == b.ClosedCases
                    && OpenCases == b.OpenCases;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Officer, ClosedCases, OpenCases);
        }
    }
}
