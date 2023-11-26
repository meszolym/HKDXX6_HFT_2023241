using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKDXX6_HFT_2023241.Models.DBModels;

namespace HKDXX6_HFT_2023241.Models.NonCrudModels
{
    public class CasesPerOfficerStatistic
    {
        public Officer Officer { get; set; }
        public int ClosedCases { get; set; }
        public int OpenCases { get; set; }

        public override bool Equals(object obj)
        {
            CasesPerOfficerStatistic b = obj as CasesPerOfficerStatistic;
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
