using CommunityToolkit.Mvvm.Messaging;
using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows.ViewModels;
using HKDXX6_GUI_2023242.WpfClient.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.Primitives;

namespace HKDXX6_GUI_2023242.WpfClient.Services
{
    public class CaseEditorViaWindow : ICaseEditor
    {
        public bool Add(FullCaseModel caseModel, IMessenger messenger)
        {
            var window = new CaseEditorPopUp(caseModel, messenger);
            return window.ShowDialog().Value;
        }

        public bool AutoAssign(FullCaseModel caseModel, IMessenger messenger)
        {
            var window = new CaseAutoAssignPopUp(caseModel, messenger);
            return window.ShowDialog().Value;
        }

        public bool Edit(FullCaseModel caseModel, IMessenger messenger)
        {
            var window = new CaseEditorPopUp(caseModel, messenger);
            return window.ShowDialog().Value;
        }

        public void ShowDetails(FullCaseModel caseModel)
        {
            var window = new CaseEditorPopUp(caseModel, null, false);
            window.ShowDialog();
        }
    }
}
