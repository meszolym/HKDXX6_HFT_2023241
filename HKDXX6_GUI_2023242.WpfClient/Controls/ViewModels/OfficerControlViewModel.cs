using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows;
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
    public class OfficerControlViewModel : ObservableRecipient, IUserControlViewModel
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

        public OfficerControlViewModel()
        {
            Officers = new RestCollection<FullOfficerModel, MinimalOfficerModel>("http://localhost:33410/", "Officer", "hub");

            EditCommand = new RelayCommand(async () =>
            {
                var window = new OfficerEditorPopUp(SelectedItem);
                if (!window.ShowDialog().Value)
                {
                    return;
                }
                try
                {
                    await Officers.Update(SelectedItem);
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
                var window = new OfficerEditorPopUp(o);
                if (!window.ShowDialog().Value)
                {
                    return;
                }
                try
                {
                    await Officers.Add(o);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

        }

        public async Task RefreshLists()
        {
            await Officers.Refresh();
        }
    }
}
