using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Models.NonCrudModels
{
    public class AutoAssignData
    {
        public int CaseID { get; set; }
        public int PrecinctID { get; set; }

        public AutoAssignData(int caseID, int precinctID)
        {
            CaseID = caseID;
            PrecinctID = precinctID;
        }
    }
}
