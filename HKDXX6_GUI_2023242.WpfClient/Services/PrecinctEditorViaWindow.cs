using CommunityToolkit.Mvvm.Messaging;
using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows;
using HKDXX6_GUI_2023242.WpfClient.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.Primitives;

namespace HKDXX6_GUI_2023242.WpfClient.Services
{
    public class PrecinctEditorViaWindow : IPrecinctEditor
    {
        public bool Add(PrecinctModel precinctModel, IMessenger messenger)
        {
            var window = new PrecinctEditorPopUp(precinctModel, messenger, true);
            return window.ShowDialog().Value;
        }

        public bool Edit(PrecinctModel precinctModel, IMessenger messenger)
        {
            var window = new PrecinctEditorPopUp(precinctModel, messenger);
            return window.ShowDialog().Value;
        }
    }
}
