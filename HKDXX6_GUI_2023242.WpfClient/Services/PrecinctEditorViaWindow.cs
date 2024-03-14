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
        public bool Add(PrecinctModel precinctModel)
        {
            var window = new PrecinctEditorPopUp(precinctModel, true);
            return window.ShowDialog().Value;
        }

        public bool Edit(PrecinctModel precinctModel)
        {
            var window = new PrecinctEditorPopUp(precinctModel);
            return window.ShowDialog().Value;
        }
    }
}
