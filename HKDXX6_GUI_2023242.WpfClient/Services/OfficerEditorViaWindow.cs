using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows;
using HKDXX6_GUI_2023242.WpfClient.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_GUI_2023242.WpfClient.Services
{
    internal class OfficerEditorViaWindow : IOfficerEditor
    {
        public bool Add(FullOfficerModel officerModel)
        {
            var window = new OfficerEditorPopUp(officerModel);
            return window.ShowDialog().Value;
        }

        public bool Edit(FullOfficerModel officerModel)
        {
            var window = new OfficerEditorPopUp(officerModel);
            return window.ShowDialog().Value;
        }
    }
}
