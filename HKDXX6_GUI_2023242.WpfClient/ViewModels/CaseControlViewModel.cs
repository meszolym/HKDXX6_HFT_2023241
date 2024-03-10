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

    public class CaseControlViewModel:ObservableRecipient
    {

        public RestCollection<FullCaseModel,MinimalCaseModel> Cases { get; set; }

        private FullCaseModel selectedItem;

        public FullCaseModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (SetProperty(ref selectedItem, value))
                {
                    (EditCommand as RelayCommand).NotifyCanExecuteChanged();
                    (DeleteCommand as RelayCommand).NotifyCanExecuteChanged();
                    (DetailsCommand as RelayCommand).NotifyCanExecuteChanged();
                    (AutoAssignCommand as RelayCommand).NotifyCanExecuteChanged();
                }
            }
        }

        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DetailsCommand { get; set; }
        public ICommand AutoAssignCommand { get; set; }

        public CaseControlViewModel()
        {

            Cases = new RestCollection<FullCaseModel, MinimalCaseModel>("http://localhost:33410/", "Case", "hub");

            EditCommand = new RelayCommand(async () =>
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
                    await Cases.Delete(SelectedItem.ID);
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
                MinimalCaseModel c = new MinimalCaseModel()
                {
                    Name = "Newly added case",
                    Description = "This is a newly added case.",
                    OpenedAt = DateTime.Now
                };
                try
                {
                    await Cases.Add(c);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            DetailsCommand = new RelayCommand(() =>
            {
                MessageBox.Show("Details");
            },
            () =>
            {
                return SelectedItem != null;
            });

            AutoAssignCommand = new RelayCommand(() =>
            {
                MessageBox.Show("AutoAssign");
            },
            () =>
            {
                return SelectedItem != null && SelectedItem.OfficerOnCaseID == null;
            });

        }
    }
}
