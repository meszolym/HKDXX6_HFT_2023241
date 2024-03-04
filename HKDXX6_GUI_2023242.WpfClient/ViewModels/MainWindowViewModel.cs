using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using HKDXX6_GUI_2023242.WpfClient.Controls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;


namespace HKDXX6_GUI_2023242.WpfClient.ViewModels
{
    public class MainWindowViewModel : ObservableRecipient
    {
        WelcomeControl welcomeControl;
        PrecinctsControl precinctsControl;
        OfficersControl officersControl;
        CasesControl casesControl;
        StatisticsControl statisticsControl;

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
        public ICommand NavToStatistics { get; set; }

        public MainWindowViewModel()
        { 
            welcomeControl = new WelcomeControl();
            precinctsControl = new PrecinctsControl();
            officersControl = new OfficersControl();
            casesControl = new CasesControl();
            statisticsControl = new StatisticsControl();
            
            CurrentControl = welcomeControl;
            
            NavToPrecincts = new RelayCommand(() =>
            {
                CurrentControl = precinctsControl;
            });

            NavToOfficers = new RelayCommand(() =>
            {
                CurrentControl = officersControl;
            });

            NavToCases = new RelayCommand(() =>
            {
                CurrentControl = casesControl;
            });

            NavToStatistics = new RelayCommand(() =>
            {
                CurrentControl = statisticsControl;
            });
        }
    }
}
