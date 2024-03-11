using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HKDXX6_GUI_2023242.WpfClient.PopUpWindows.ViewModels
{
    public class CaseAutoAssignPopUpViewModel
    {
        public List<PrecinctModel> Precincts { get; private set; }

        public string CaseName { get; private set; }

        public PrecinctModel SelectedItem {
            get;
            set;
        }

        public CaseAutoAssignPopUpViewModel()
        {
            Precincts = new RestService("http://localhost:33410/", "Precinct").Get<PrecinctModel>("Precinct");
        }

        public void Init(string CaseName)
        {
            this.CaseName = CaseName;
        }
    }
}
