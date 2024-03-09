﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HKDXX6_GUI_2023242.WpfClient.PopUpWindows;
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
        public RestCollection<Case> Cases { get; set; }

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
            Cases = new RestCollection<Case>("http://localhost:33410/", "Case", "hub");

            EditCommand = new RelayCommand(async() =>
            {
                var window = new EditCaseWindow(selectedItem);
                if (window.ShowDialog().Value)
                {
                    try
                    {
                        if (selectedItem.OfficerOnCaseID != selectedItem.OfficerOnCase.BadgeNo)
                        {
                            selectedItem.OfficerOnCaseID = selectedItem.OfficerOnCase.BadgeNo;
                        }
                        await Cases.Update(selectedItem);
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

            DeleteCommand = new RelayCommand(async() =>
            {
                try
                {
                    await Cases.Delete(selectedItem.ID);
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
                MessageBox.Show("Add");
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
