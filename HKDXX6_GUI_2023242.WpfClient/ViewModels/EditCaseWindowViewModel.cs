using CommunityToolkit.Mvvm.Input;
using HKDXX6_HFT_2023241.Models.DBModels;
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
        RestCollection<Officer> officers;
        public RestCollection<Officer> Officers { 
            get
            {
                return officers;
            }
        }

        public Case Case { get; set; }

        public ICommand SaveCommand { get; set; }

        public EditCaseWindowViewModel(Case @case)
        {
            officers = new RestCollection<Officer>("http://localhost:33410/", "officer", "hub");
            Case = @case;
        }
    }
}
