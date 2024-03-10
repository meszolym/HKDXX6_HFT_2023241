using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_GUI_2023242.WpfClient.PopUpWindows.ViewModels
{
    public class CaseAutoAssignPopUpViewModel
    {
        public List<PrecinctModel> Precincts { get; private set; }

        public string CaseName { get; private set; }

        public PrecinctModel? Precinct { get; set; }

        public CaseAutoAssignPopUpViewModel()
        {
            Precincts = new RestService("http://localhost:33410/", "Precinct").Get<PrecinctModel>("Precinct");
        }

        public void Init(string CaseName, ref PrecinctModel? p)
        {
            this.CaseName = CaseName;
            this.Precinct = p;
        }
    }
}
