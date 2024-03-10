using HKDXX6_GUI_2023242.WpfClient.APIModels;
using HKDXX6_GUI_2023242.WpfClient.PopUpWindows.ViewModels;
using HKDXX6_GUI_2023242.WpfClient.Tools;
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
    /// Interaction logic for OfficerEditorPopUp.xaml
    /// </summary>
    public partial class OfficerEditorPopUp : Window
    {

        OfficerEditorPopUpViewModel viewModel;

        public OfficerEditorPopUp(FullOfficerModel o)
        {
            InitializeComponent();
            viewModel = new OfficerEditorPopUpViewModel();
            this.DataContext = viewModel;
            viewModel.Init(o);
        }

        private void cb_Precinct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_DCO.SelectedItem == null || cb_Precinct.SelectedItem == null)
            {
                e.Handled = true;
                return;
            }
            if ((cb_DCO.SelectedItem as FullOfficerModel).PrecinctID != (cb_Precinct.SelectedItem as PrecinctModel).ID)
            {
                cb_DCO.SelectedItem = null;
            }
            
            e.Handled = true;
        }

        private void cb_DCO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_DCO.SelectedItem == null || cb_Precinct.SelectedItem == null)
            {
                e.Handled = true;
                return;
            }
            if ((cb_DCO.SelectedItem as FullOfficerModel).PrecinctID != (cb_Precinct.SelectedItem as PrecinctModel).ID)
            {
                cb_Precinct.SelectedItem = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in grid.Children)
            {
                if (item is TextBox tb)
                {
                    tb.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                    continue;
                }
                if (item is DatePicker dp)
                {
                    dp.GetBindingExpression(DatePicker.TextProperty).UpdateSource();
                    continue;
                }
                if (item is ComboBox cmb)
                {
                    cmb.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
                    continue;
                }
            }

            this.DialogResult = true;
        }
    }
}
