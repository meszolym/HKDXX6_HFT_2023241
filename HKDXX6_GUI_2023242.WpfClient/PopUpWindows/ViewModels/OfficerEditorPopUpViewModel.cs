using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_GUI_2023242.WpfClient.PopUpWindows.ViewModels
{
    public class OfficerEditorPopUpViewModel
    {
        public FullOfficerModel Officer { get; set; }

        public List<Ranks> Ranks { get; private set; }
        public List<PrecinctModel> Precincts { get; private set; }
        public List<FullOfficerModel> Officers { get; private set; }

        public OfficerEditorPopUpViewModel()
        {
            Ranks = Enum.GetValues(typeof(Ranks)).Cast<Ranks>().ToList();
            Precincts = new RestService("http://localhost:33410/", "Precinct").Get<PrecinctModel>("Precinct");
        }

        public void Init(FullOfficerModel o)
        {
            Officer = o;
            Officers = new RestService("http://localhost:33410/", "Officer").Get<FullOfficerModel>("Officer").Where(t => t.BadgeNo != Officer.BadgeNo).ToList();
        }



    }
}
