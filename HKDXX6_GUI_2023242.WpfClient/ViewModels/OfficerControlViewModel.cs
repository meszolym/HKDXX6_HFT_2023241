using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HKDXX6_GUI_2023242.WpfClient.ViewModels
{
    public class OfficerControlViewModel:ObservableRecipient
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

            EditCommand = new RelayCommand(() => 
            {
                MessageBox.Show("Edit");
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

            AddCommand = new RelayCommand(() =>
            {
                MessageBox.Show("Add");
            });

        }
    }
}
