using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows.ViewModels;
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

    public class CaseControlViewModel : ObservableRecipient
    {

        public RestCollection<FullCaseModel, MinimalCaseModel> Cases { get; set; }

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
                var window = new CaseEditorPopUp(SelectedItem);
                if (!window.ShowDialog().Value)
                {
                    return;
                }
                try
                {
                    await Cases.Update(SelectedItem);
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
                var c = new FullCaseModel();
                c.OpenedAt = DateTime.Now;
                var window = new CaseEditorPopUp(c);
                if (!window.ShowDialog().Value)
                {
                    return;
                }
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
                var window = new CaseEditorPopUp(SelectedItem, false);
                window.ShowDialog();
            },
            () =>
            {
                return SelectedItem != null;
            });

            AutoAssignCommand = new RelayCommand(() =>
            {
                var AssignModel = new AutoAssignCaseModel();
                AssignModel.CaseID = SelectedItem.ID;
                var window = new CaseAutoAssignPopUp(SelectedItem.Name);
                
                if (!window.ShowDialog().Value)
                {
                    return;
                }
                try
                {
                    if ((window.DataContext as CaseAutoAssignPopUpViewModel).SelectedItem == null)
                    {
                        throw new ArgumentException("No precinct chosen!");
                    }
                    AssignModel.PrecinctID = (window.DataContext as CaseAutoAssignPopUpViewModel).SelectedItem.ID;
                    new RestService("http://localhost:33410/", "Case").Post(AssignModel, "/Case/AutoAssign");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            },
            () =>
            {
                return SelectedItem != null && SelectedItem.OfficerOnCaseID == null;
            });

        }
    }
}
