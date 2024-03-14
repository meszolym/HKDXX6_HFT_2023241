using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows.ViewModels;
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
        public bool Add(FullCaseModel caseModel)
        {
            var window = new CaseEditorPopUp(caseModel);
            return window.ShowDialog().Value;
        }

        public AutoAssignCaseModel? AutoAssign(FullCaseModel caseModel)
        {
            var AssignModel = new AutoAssignCaseModel();
            AssignModel.CaseID = caseModel.ID;
            var window = new CaseAutoAssignPopUp(caseModel.Name);
            if (!window.ShowDialog().Value)
            {
                return null;
            }

            if ((window.DataContext as CaseAutoAssignPopUpViewModel).SelectedItem == null)
            {
                throw new ArgumentException("No precinct chosen!");
            }
            AssignModel.PrecinctID = (window.DataContext as CaseAutoAssignPopUpViewModel).SelectedItem.ID;
            return AssignModel;
        }

        public bool Edit(FullCaseModel caseModel)
        {
            var window = new CaseEditorPopUp(caseModel);
            return window.ShowDialog().Value;
        }
    }
}
