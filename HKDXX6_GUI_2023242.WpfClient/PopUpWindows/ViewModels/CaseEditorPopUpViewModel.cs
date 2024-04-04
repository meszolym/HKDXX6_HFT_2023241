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
    public class CaseEditorPopUpViewModel
    {
        public List<FullOfficerModel> Officers { get; private set; }

        public ICommand CloseCommand { get; set; }

        public bool Edit { get; private set; }

        private int id;
        public string Name { get; set; }
        public DateTime OpenedAt { get; set; }
        public FullOfficerModel OfficerOnCase { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string Description { get; set; }

        public CaseEditorPopUpViewModel()
        {
            Officers = new RestService("http://localhost:33410/", "Officer").Get<FullOfficerModel>("Officer");
        }

        public void Init(FullCaseModel c, bool edit, Action CloseAction, IMessenger messenger)
        {
            id = c.ID;
            Name = c.Name;
            OpenedAt = c.OpenedAt;
            OfficerOnCase = c.OfficerOnCase;
            ClosedAt = c.ClosedAt;
            Description = c.Description;

            Edit = edit;

            CloseCommand = new RelayCommand(() =>
            {
                messenger.Send(new FullCaseModel()
                {
                    ID = id,
                    Name = Name,
                    OpenedAt = OpenedAt,
                    OfficerOnCaseID = (OfficerOnCase == null ? null : OfficerOnCase.BadgeNo),
                    ClosedAt = ClosedAt,
                    Description = Description
                }, "CaseUpdateOrAddDone");
                CloseAction();
            });
        }
    }
}
