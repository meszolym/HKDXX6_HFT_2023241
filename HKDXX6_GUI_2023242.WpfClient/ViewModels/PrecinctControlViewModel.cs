using CommunityToolkit.Mvvm.ComponentModel;
using HKDXX6_HFT_2023241.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_GUI_2023242.WpfClient.ViewModels
{
    class PrecinctControlViewModel:ObservableRecipient
    {
        RestCollection<Precinct> precincts;

        public RestCollection<Precinct> Precincts { get; set; }

        public PrecinctControlViewModel()
        {
            Precincts = new RestCollection<Precinct>("http://localhost:33410/", "precinct");
        }
    }
}
