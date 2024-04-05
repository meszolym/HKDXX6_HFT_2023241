using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows.ViewModels;
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

        ICaseEditor editor;

        private FullCaseModel ItemAddUpdate;
        private AutoAssignCaseModel ItemAutoAssign;

        public CaseControlViewModel()
        {

            Cases = new RestCollection<FullCaseModel, MinimalCaseModel>("http://localhost:33410/", "Case", "hub");

            if (editor == null)
            {
                editor = Ioc.Default.GetService<ICaseEditor>();
            }

            Messenger.Register<CaseControlViewModel, FullCaseModel, string>(this, "CaseUpdateOrAddDone", (rec, msg) =>
            {
                ItemAddUpdate = msg;
            });

            Messenger.Register<CaseControlViewModel, AutoAssignCaseModel, string>(this, "CaseAutoAssignDone", (rec, msg) =>
            {
                ItemAutoAssign = msg;
            });

            EditCommand = new RelayCommand(async () =>
            {
                if (!editor.Edit(SelectedItem, Messenger))
                {
                    return;
                }
                try
                {
                    SelectedItem.ClosedAt = ItemAddUpdate.ClosedAt;
                    SelectedItem.Description = ItemAddUpdate.Description;
                    SelectedItem.ID = ItemAddUpdate.ID;
                    SelectedItem.Name = ItemAddUpdate.Name;
                    SelectedItem.OfficerOnCaseID = ItemAddUpdate.OfficerOnCaseID;
                    SelectedItem.OpenedAt = ItemAddUpdate.OpenedAt;


                    await Cases.Update(SelectedItem);
                }
                catch (Exception ex)
                {
                    await Cases.Init();
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
                
                if (!editor.Add(c, Messenger))
                {
                    return;
                }
                try
                {
                    await Cases.Add(ItemAddUpdate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            DetailsCommand = new RelayCommand(() =>
            {
                editor.ShowDetails(SelectedItem);
            },
            () =>
            {
                return SelectedItem != null;
            });

            AutoAssignCommand = new RelayCommand(() =>
            {
                try
                {
                    
                    if (!editor.AutoAssign(SelectedItem, Messenger))
                    {
                        return;
                    }
                    try
                    {
                        new RestService("http://localhost:33410/", "Case").Post(ItemAutoAssign, "/Case/AutoAssign");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
