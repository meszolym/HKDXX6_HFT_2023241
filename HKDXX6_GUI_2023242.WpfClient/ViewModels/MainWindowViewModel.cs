using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using HKDXX6_GUI_2023242.WpfClient.Controls;
using HKDXX6_GUI_2023242.WpfClient.Controls.ViewModels;
using HKDXX6_GUI_2023242.WpfClient.Services.Interfaces;


namespace HKDXX6_GUI_2023242.WpfClient.ViewModels
{
    public class MainWindowViewModel : ObservableRecipient
    {

        UserControl currentControl;
        public UserControl CurrentControl
        {
            get
            {
                return currentControl;
            }
            set
            {
                SetProperty(ref currentControl, value);
            }
        }

        public ICommand NavToPrecincts { get; set; }
        public ICommand NavToCases { get; set; }
        public ICommand NavToOfficers { get; set; }

        IControlsNavigator navigator;

        public MainWindowViewModel()
        { 
            
            if (navigator == null)
            {
                navigator = Ioc.Default.GetService<IControlsNavigator>();
            }

            CurrentControl = navigator.WelcomeControl;
            
            NavToPrecincts = new RelayCommand(() =>
            {
                CurrentControl = navigator.PrecinctsControl;
            });

            NavToOfficers = new RelayCommand(() =>
            {
                CurrentControl = navigator.OfficersControl;
            });

            NavToCases = new RelayCommand(() =>
            {
                CurrentControl = navigator.CasesControl;
            });
        }
    }
}
