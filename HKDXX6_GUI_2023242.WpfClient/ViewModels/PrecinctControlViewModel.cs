﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public class PrecinctControlViewModel:ObservableRecipient
    {
        RestCollection<Precinct> precincts;

        public RestCollection<Precinct> Precincts { get; set; }

        private Precinct selectedItem;

        public Precinct SelectedItem
        {
            get { return selectedItem; }
            set 
            {
                if(SetProperty(ref selectedItem, value))
                {
                    (EditCommand as RelayCommand).NotifyCanExecuteChanged();
                    (DeleteCommand as RelayCommand).NotifyCanExecuteChanged();
                    (DetailsCommand as RelayCommand).NotifyCanExecuteChanged();
                }
            }
        }


        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DetailsCommand { get; set; }

        public PrecinctControlViewModel()
        {
            Precincts = new RestCollection<Precinct>("http://localhost:33410/", "precinct", "hub");

            EditCommand = new RelayCommand(() =>
            {
                MessageBox.Show("Edit");
            },
            () =>
            {
                return SelectedItem != null;
            });

            DeleteCommand = new RelayCommand(() =>
            {
                try
                {
                    Precincts.Delete(selectedItem.ID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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

            DetailsCommand = new RelayCommand(() =>
            {
                MessageBox.Show("Details");
            },
            () =>
            {
                return SelectedItem != null;
            });
        }
    }
}
