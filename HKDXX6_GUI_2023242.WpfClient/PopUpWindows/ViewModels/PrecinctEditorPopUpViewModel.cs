using HKDXX6_GUI_2023242.WpfClient.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_GUI_2023242.WpfClient.PopUpWindows.ViewModels
{
    public class PrecinctEditorPopUpViewModel
    {
        public PrecinctModel Precinct { get; set; }

        public void Init(PrecinctModel p)
        {
            Precinct = p;
        }

        public PrecinctEditorPopUpViewModel()
        {

        }
    }
}
