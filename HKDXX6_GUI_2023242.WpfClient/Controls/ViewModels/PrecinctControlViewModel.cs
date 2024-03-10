﻿using CommunityToolkit.Mvvm.ComponentModel;
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
    public class PrecinctControlViewModel : ObservableRecipient
    {

        RestCollection<PrecinctModel, PrecinctModel> precincts;

        public RestCollection<PrecinctModel, PrecinctModel> Precincts { get; set; }

        private PrecinctModel selectedItem;

        public PrecinctModel SelectedItem
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

        public PrecinctControlViewModel()
        {
            Precincts = new RestCollection<PrecinctModel, PrecinctModel>("http://localhost:33410/", "Precinct", "hub");

            EditCommand = new RelayCommand(async () =>
            {
                var window = new PrecinctEditorPopUp(SelectedItem);
                if (!window.ShowDialog().Value)
                {
                    return;
                }
                try
                {
                    await Precincts.Update(SelectedItem);
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
                    await Precincts.Delete(SelectedItem.ID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    await Precincts.Refresh();
                }
            },
            () =>
            {
                return SelectedItem != null;
            });

            AddCommand = new RelayCommand(async () =>
            {
                var p = new PrecinctModel();
                var window = new PrecinctEditorPopUp(p, true);
                if (!window.ShowDialog().Value)
                {
                    return;
                }
                try
                {
                    await Precincts.Add(p);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
    }
}