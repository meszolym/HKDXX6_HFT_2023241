using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_GUI_2023242.WpfClient.PopUpWindows.ViewModels
{
    public class CaseEditorPopUpViewModel
    {
        public List<FullOfficerModel> Officers { get; private set; }
        public FullCaseModel Case { get; set; }

        public CaseEditorPopUpViewModel()
        {
            Officers = new RestService("http://localhost:33410/", "Officer").Get<FullOfficerModel>("Officer");
        }

        public void Init(FullCaseModel c)
        {
            Case = c;
        }
    }
}
