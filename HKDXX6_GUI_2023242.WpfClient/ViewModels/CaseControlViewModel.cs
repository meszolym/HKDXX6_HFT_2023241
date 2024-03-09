using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows;
using HKDXX6_GUI_2023242.WpfClient.Tools;
using HKDXX6_HFT_2023241.Models.DBModels;
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

        public List<Case> Cases { get; set; }

        private Case selectedItem;

        public Case SelectedItem
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
            Cases = new List<Case>();

            EditCommand = new RelayCommand(() =>
            {
                var window = new EditCaseWindow(selectedItem);
                if (window.ShowDialog().Value)
                {
                    selectedItem.OfficerOnCaseID = selectedItem.OfficerOnCase.BadgeNo;
                    try
                    {
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            },
            () =>
            {
                return SelectedItem != null;
            });

            DeleteCommand = new RelayCommand(() =>
            {
                try
                {
                    
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
                Case c = new Case(0,"New case","This is a newly added case.",DateTime.Now);
                try
                {
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            DetailsCommand = new RelayCommand(() =>
            {
                var window = new EditCaseWindow(selectedItem,false);
                window.ShowDialog();
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
