using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows;
using HKDXX6_GUI_2023242.WpfClient.Services.Interfaces;
using HKDXX6_GUI_2023242.WpfClient.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HKDXX6_GUI_2023242.WpfClient.Controls.ViewModels
{
    public class OfficerControlViewModel : ObservableRecipient
    {
        public RestCollection<FullOfficerModel, MinimalOfficerModel> Officers { get; set; }

        private FullOfficerModel selectedItem;

        public FullOfficerModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (SetProperty(ref selectedItem, value))
                {
                    (EditCommand as RelayCommand).NotifyCanExecuteChanged();
                    (DeleteCommand as RelayCommand).NotifyCanExecuteChanged();
                }
            }
        }

        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand AddCommand { get; set; }

        IOfficerEditor editor;

        private FullOfficerModel ItemAddUpdate;

        public OfficerControlViewModel()
        {
            Officers = new RestCollection<FullOfficerModel, MinimalOfficerModel>("http://localhost:33410/", "Officer", "hub");

            Messenger.Register<OfficerControlViewModel, FullOfficerModel, string>(this, "OfficerUpdateOrAddDone", (rec, msg) =>
            {
                ItemAddUpdate = msg;
            });

            if (editor == null)
            {
                editor = Ioc.Default.GetService<IOfficerEditor>();
            }

            EditCommand = new RelayCommand(async () =>
            {
                if (!editor.Edit(SelectedItem,Messenger))
                {
                    return;
                }
                try
                {
                    SelectedItem.BadgeNo = ItemAddUpdate.BadgeNo;
                    SelectedItem.DirectCO_BadgeNo = ItemAddUpdate.DirectCO_BadgeNo;
                    SelectedItem.FirstName = ItemAddUpdate.FirstName;
                    SelectedItem.HireDate = ItemAddUpdate.HireDate;
                    SelectedItem.LastName = ItemAddUpdate.LastName;
                    SelectedItem.PrecinctID = ItemAddUpdate.PrecinctID;
                    SelectedItem.Rank = ItemAddUpdate.Rank;
                    await Officers.Update(SelectedItem);
                }
                catch (Exception ex)
                {
                    await Officers.Init();
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                }
            },
            () =>
            {
                return SelectedItem != null;
            });

            DeleteCommand = new RelayCommand(async () =>
            {
                try
                {
                    await Officers.Delete(SelectedItem.BadgeNo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            },
            () =>
            {
                return SelectedItem != null;
            });

            AddCommand = new RelayCommand(async () =>
            {
                var o = new FullOfficerModel();
                o.HireDate = DateTime.Today;
                if (!editor.Add(o, Messenger))
                {
                    return;
                }
                try
                {
                    await Officers.Add(ItemAddUpdate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

        }
    }
}
