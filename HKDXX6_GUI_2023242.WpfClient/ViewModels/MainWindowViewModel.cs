using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HKDXX6_GUI_2023242.WpfClient.Controls;
using HKDXX6_GUI_2023242.WpfClient.Controls.ViewModels;


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
        //public ICommand NavToStatistics { get; set; }

        public MainWindowViewModel()
        { 
            welcomeControl = new WelcomeControl();
            precinctsControl = new PrecinctsControl();
            officersControl = new OfficersControl();
            casesControl = new CasesControl();
            //statisticsControl = new StatisticsControl();
            
            CurrentControl = welcomeControl;
            
            NavToPrecincts = new RelayCommand(() =>
            {
                (precinctsControl.DataContext as IUserControlViewModel).RefreshLists();
                CurrentControl = precinctsControl;
            });

            NavToOfficers = new RelayCommand(() =>
            {
                (officersControl.DataContext as IUserControlViewModel).RefreshLists();
                CurrentControl = officersControl;
            });

            NavToCases = new RelayCommand(() =>
            {
                (casesControl.DataContext as IUserControlViewModel).RefreshLists();
                CurrentControl = casesControl;
            });

            /*NavToStatistics = new RelayCommand(() =>
            {
                CurrentControl = statisticsControl;
            });*/
        }
    }
}
