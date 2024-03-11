using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HKDXX6_GUI_2023242.WpfClient.PopUpWindows
{
    /// <summary>
    /// Interaction logic for CaseAutoAssignPopUp.xaml
    /// </summary>
    public partial class CaseAutoAssignPopUp : Window
    {
        public CaseAutoAssignPopUp(string CaseName)
        {
            InitializeComponent();
            var viewModel = new CaseAutoAssignPopUpViewModel();
            this.DataContext = viewModel;
            viewModel.Init(CaseName);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
