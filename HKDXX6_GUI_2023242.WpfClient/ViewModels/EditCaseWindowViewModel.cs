using CommunityToolkit.Mvvm.Input;
using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.Tools;
using HKDXX6_GUI_2023242.WpfClient.Tools.MovieDbApp.RestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HKDXX6_GUI_2023242.WpfClient.ViewModels
{
    public class EditCaseWindowViewModel
    {
        RestCollection<FullOfficerModel,MinimalOfficerModel> officers;
        public RestCollection<FullOfficerModel, MinimalOfficerModel> Officers { 
            get
            {
                return officers;
            }
        }

        public FullCaseModel Case { get; set; }

        public ICommand SaveCommand { get; set; }

        public EditCaseWindowViewModel(FullCaseModel @case)
        {
            officers = new RestCollection<FullOfficerModel, MinimalOfficerModel>("http://localhost:33410/", "Officer", "hub");
            Case = @case;
        }
    }
}
