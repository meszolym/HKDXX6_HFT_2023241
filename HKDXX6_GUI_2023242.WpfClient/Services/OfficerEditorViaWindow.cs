using CommunityToolkit.Mvvm.Messaging;
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
    public class OfficerEditorViaWindow : IOfficerEditor
    {
        public bool Add(FullOfficerModel officerModel, IMessenger messenger)
        {
            var window = new OfficerEditorPopUp(officerModel, messenger);
            return window.ShowDialog().Value;
        }

        public bool Edit(FullOfficerModel officerModel, IMessenger messenger)
        {
            var window = new OfficerEditorPopUp(officerModel, messenger);
            return window.ShowDialog().Value;
        }
    }
}
