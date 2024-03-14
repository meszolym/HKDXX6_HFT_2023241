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
            PrecinctModel precinct = new PrecinctModel();
            var window = new CaseAutoAssignPopUp(caseModel.Name, AssignModel);
            if (!window.ShowDialog().Value)
            {
                return null;
            }

            return AssignModel;
        }

        public bool Edit(FullCaseModel caseModel)
        {
            var window = new CaseEditorPopUp(caseModel);
            return window.ShowDialog().Value;
        }
    }
}
