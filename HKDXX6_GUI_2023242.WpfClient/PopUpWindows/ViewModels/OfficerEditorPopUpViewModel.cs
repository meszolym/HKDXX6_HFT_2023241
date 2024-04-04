using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HKDXX6_GUI_2023242.WpfClient.PopUpWindows.ViewModels
{
    public class OfficerEditorPopUpViewModel
    {
        int id;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime HireDate { get; set; }
        public PrecinctModel Precinct {get; set;}
        public FullOfficerModel DirectCO { get; set; }
        public Ranks Rank { get; set; }


        public List<Ranks> Ranks { get; private set; }
        public List<PrecinctModel> Precincts { get; private set; }
        public List<FullOfficerModel> Officers { get; private set; }

        public ICommand CloseCommand { get; set; }

        public OfficerEditorPopUpViewModel()
        {
            Ranks = Enum.GetValues(typeof(Ranks)).Cast<Ranks>().ToList();
            Precincts = new RestService("http://localhost:33410/", "Precinct").Get<PrecinctModel>("Precinct");
        }

        public void Init(FullOfficerModel o, Action CloseAction, IMessenger messenger)
        {
            id = o.BadgeNo;
            FirstName = o.FirstName;
            LastName = o.LastName;
            HireDate = o.HireDate;
            Precinct = o.Precinct;
            DirectCO = o.DirectCO;
            Rank = o.Rank;


            Officers = new RestService("http://localhost:33410/", "Officer").Get<FullOfficerModel>("Officer").Where(t => t.BadgeNo != o.BadgeNo).ToList();

            CloseCommand = new RelayCommand(() =>
            {
                messenger.Send(new FullOfficerModel(){
                    BadgeNo=id,
                    FirstName = FirstName,
                    LastName = LastName,
                    HireDate = HireDate,
                    Rank = Rank,
                    PrecinctID = Precinct.ID,
                    DirectCO_BadgeNo = (DirectCO == null ? null : DirectCO.BadgeNo)
                    }, "OfficerUpdateOrAddDone");
                CloseAction();
            });

        }



    }
}
