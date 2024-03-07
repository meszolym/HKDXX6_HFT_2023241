using CommunityToolkit.Mvvm.ComponentModel;
using HKDXX6_HFT_2023241.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_GUI_2023242.WpfClient.ViewModels
{
    class OfficerControlViewModel:ObservableRecipient
    {
        RestCollection<Officer> officers;

        public RestCollection<Officer> Officers { get; set; }

        public OfficerControlViewModel()
        {
            Officers = new RestCollection<Officer>("http://localhost:33410/", "officer");
        }
    }
}
