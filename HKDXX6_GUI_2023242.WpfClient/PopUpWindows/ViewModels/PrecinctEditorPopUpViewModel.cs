using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HKDXX6_GUI_2023242.WpfClient.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HKDXX6_GUI_2023242.WpfClient.PopUpWindows.ViewModels
{
    public class PrecinctEditorPopUpViewModel
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public ICommand CloseCommand { get; set; }

        public bool IdEnabled { get; private set; }

        IMessenger messenger;

        public void Init(PrecinctModel p, bool addition, Action closeAction, IMessenger messenger)
        {

            Id = p.ID;
            Address = p.Address;
            
            IdEnabled = addition;
            this.messenger = messenger;
            CloseCommand = new RelayCommand(() =>
            {
                messenger.Send(new PrecinctModel() { ID = Id, Address = Address}, "PrecinctUpdateOrAddDone");
                closeAction();
            });
        }

        public PrecinctEditorPopUpViewModel()
        {

        }
    }
}
